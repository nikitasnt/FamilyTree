using FamilyTree.Database;
using FamilyTree.Database.Models;
using FamilyTree.Interfaces;
using FamilyTree.Models.Commands;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Services;

public class FamilyMemberService(FamilyTreeDbContext context) : IFamilyMemberService
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
            HierarchyPath = new LTree() // Currently empty.
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
}