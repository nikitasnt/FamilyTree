using FamilyTree.Database;
using FamilyTree.Database.Models;
using FamilyTree.Interfaces;
using FamilyTree.Models;
using FamilyTree.Models.Commands;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Services;

public class FamilyTreeService(FamilyTreeDbContext context) : IFamilyTreeService
{
    private async Task SaveNewFamilyMember(NewFamilyMemberCmd cmd, FamilyMember? parent = null)
    {
        // Using a transaction because you want to perform inserts and updates atomically.
        await using var tr = await context.Database.BeginTransactionAsync();
        
        // Saving a new entity.
        var newFamilyMember = new FamilyMember
        {
            Firstname = cmd.Firstname,
            Lastname = cmd.Lastname,
            Birthday = cmd.Birthday,
            HierarchyPath = new LTree(string.Empty) // Currently empty.
        };
        await context.AddAsync(newFamilyMember);
        await context.SaveChangesAsync();
        
        // Setting the correct value with generated ID by DB.
        newFamilyMember.HierarchyPath =
            new LTree(parent != null ? $"{parent.HierarchyPath}.{newFamilyMember.Id}" : newFamilyMember.Id.ToString());

        // Update the new entity with the correct HierarchyPath value.
        context.Update(newFamilyMember);
        await context.SaveChangesAsync();

        await tr.CommitAsync();
    }
    
    public Task CreateNewMemberAsync(NewFamilyMemberCmd cmd)
    {
        return SaveNewFamilyMember(cmd);
    }

    public async Task CreateNewMemberAsync(NewFamilyMemberCmd cmd, int parentId)
    {
        var parentFamilyMember = await context.FamilyMembers.FindAsync(parentId);

        if (parentFamilyMember == null)
        {
            throw new BadHttpRequestException($"No family member with ID {parentId} for using as parent.");
        }

        await SaveNewFamilyMember(cmd, parentFamilyMember);
    }

    private static IEnumerable<FamilyMemberWithChildrenInfo> GetChildren(FamilyMember parentMember,
        IReadOnlyList<FamilyMember> allMembers)
    {
        return allMembers.Where(m => m.ParentId == parentMember.Id).Select(member => new FamilyMemberWithChildrenInfo
        {
            Id = member.Id,
            Firstname = member.Firstname,
            Lastname = member.Lastname,
            Birthday = member.Birthday,
            DirectChildren = GetChildren(member, allMembers)
        });
    }

    public async IAsyncEnumerable<FamilyMemberWithChildrenInfo> GetAllMembers()
    {
        var allMembers = await context.FamilyMembers
            .AsNoTracking()
            .Select(m => new FamilyMember
            {
                Id = m.Id,
                Firstname = m.Firstname,
                Lastname = m.Lastname,
                Birthday = m.Birthday,
                HierarchyPath = m.HierarchyPath,
                HierarchyLevel = m.HierarchyPath.NLevel,
                ParentId = m.HierarchyPath.NLevel == 1
                    ? null
                    : int.Parse(m.HierarchyPath.Subpath(m.HierarchyPath.NLevel - 2, 1))
            })
            .ToListAsync();
        
        // Bypassing the very first (top) parents.
        foreach (var topMember in allMembers.Where(m => m.HierarchyLevel == 1))
        {
            yield return new FamilyMemberWithChildrenInfo
            {
                Id = topMember.Id,
                Firstname = topMember.Firstname,
                Lastname = topMember.Lastname,
                Birthday = topMember.Birthday,
                DirectChildren = GetChildren(topMember, allMembers)
            };
        }
    }

    public async Task<FamilyMemberInfo?> GetGreatGrandfather(int greatGrandsonId)
    {
        return await context.FamilyMembers
            .AsNoTracking()
            .Where(m => m.HierarchyPath ==
                        // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
                        context.FamilyMembers.Single(m1 => m1.Id == greatGrandsonId).HierarchyPath.Subpath(0, -3))
            .Select(m => new FamilyMemberInfo
            {
                Id = m.Id,
                Firstname = m.Firstname,
                Lastname = m.Lastname,
                Birthday = m.Birthday
            })
            .SingleOrDefaultAsync();
    }

    public async Task<FamilyMemberInfo?> GetGrandfather(int grandsonId)
    {
        return await context.FamilyMembers
            .AsNoTracking()
            .Where(m => m.HierarchyPath ==
                        // ReSharper disable once EntityFramework.UnsupportedServerSideFunctionCall
                        context.FamilyMembers.Single(m1 => m1.Id == grandsonId).HierarchyPath.Subpath(0, -2))
            .Select(m => new FamilyMemberInfo
            {
                Id = m.Id,
                Firstname = m.Firstname,
                Lastname = m.Lastname,
                Birthday = m.Birthday
            })
            .SingleOrDefaultAsync();
    }
}