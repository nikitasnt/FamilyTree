using FamilyTree.Models;
using FamilyTree.Models.Commands;

namespace FamilyTree.Interfaces;

public interface IFamilyTreeService
{
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd);
    
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd, int parentId);

    IAsyncEnumerable<FamilyMemberInfo> GetAllMembers();
    
    
}