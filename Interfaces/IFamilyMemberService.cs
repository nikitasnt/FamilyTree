using FamilyTree.Models.Commands;

namespace FamilyTree.Interfaces;

public interface IFamilyMemberService
{
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd);
    
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd, int parentId);
}