using FamilyTree.Database;
using FamilyTree.Database.Models;
using FamilyTree.Interfaces;
using FamilyTree.Models;
using FamilyTree.Models.Commands;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Services;

public class FamilyTreeService(FamilyTreeDbContext context) : IFamilyTreeService
{
    private async Task SaveNewFamilyMember(NewFamilyMemberCmd cmd, int? parentId = null)
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
            new LTree(parentId.HasValue ? $"{parentId}.{newFamilyMember.Id}" : newFamilyMember.Id.ToString());

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

        await SaveNewFamilyMember(cmd, parentFamilyMember.Id);
    }

    private static IEnumerable<FamilyMemberInfo> GetChildren(FamilyMember parentMember,
        IReadOnlyDictionary<int, FamilyMember> allMembers)
    {
        return allMembers.Values.Where(m => m.ParentId == parentMember.Id).Select(member => new FamilyMemberInfo
        {
            Id = member.Id,
            Firstname = member.Firstname,
            Lastname = member.Lastname,
            Birthday = member.Birthday,
            DirectChildren = GetChildren(member, allMembers)
        });
    }

    public async IAsyncEnumerable<FamilyMemberInfo> GetAllMembers()
    {
        var allMembers = await context.FamilyMembers
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
            .ToDictionaryAsync(m => m.Id);
        
        // Обход самый первых (верхних) родителей.
        foreach (var topMember in allMembers.Values.Where(m => m.HierarchyLevel == 1))
        {
            yield return new FamilyMemberInfo
            {
                Id = topMember.Id,
                Firstname = topMember.Firstname,
                Lastname = topMember.Lastname,
                Birthday = topMember.Birthday,
                DirectChildren = GetChildren(topMember, allMembers)
            };
        }
    }
}