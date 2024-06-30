using FamilyTree.Models;
using FamilyTree.Models.Commands;

namespace FamilyTree.Interfaces;

public interface IFamilyTreeService
{
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd);
    
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd, int parentId);

    Task<IEnumerable<FamilyMemberWithChildrenInfo>> GetAllMembers();

    Task<FamilyMemberInfo?> GetGreatGrandfather(int greatGrandsonId);
    
    Task<FamilyMemberInfo?> GetGrandfather(int grandsonId);

    Task<IEnumerable<FamilyMemberInfo>> GetGreatGrandsons(int greatGrandfatherId);
}