using FamilyTree.Models;
using FamilyTree.Models.Commands;

namespace FamilyTree.Interfaces;

public interface IFamilyTreeService
{
    /// <summary>
    /// Create a new family member without a parent.
    /// </summary>
    /// <param name="cmd">Family member information.</param>
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd);

    /// <summary>
    /// Create a new family member with a parent.
    /// </summary>
    /// <param name="cmd">Family member information.</param>
    /// <param name="parentId">Parent ID.</param>
    Task CreateNewMemberAsync(NewFamilyMemberCmd cmd, int parentId);

    /// <summary>
    /// Get all the people in the form of trees.
    /// </summary>
    Task<IEnumerable<FamilyMemberWithChildrenInfo>> GetAllMembersAsync();

    /// <summary>
    /// Get great-grandfather by person ID.
    /// </summary>
    /// <param name="greatGrandsonId">Person ID (great-grandson).</param>
    Task<FamilyMemberInfo?> GetGreatGrandfatherAsync(int greatGrandsonId);
    
    /// <summary>
    /// Get grandfather by person ID.
    /// </summary>
    /// <param name="grandsonId">Person ID (grandson).</param>
    Task<FamilyMemberInfo?> GetGrandfatherAsync(int grandsonId);

    /// <summary>
    /// Get great-grandsons by person ID.
    /// </summary>
    /// <param name="greatGrandfatherId">Person ID (great-grandfather).</param>
    Task<IEnumerable<FamilyMemberInfo>> GetGreatGrandsonsAsync(int greatGrandfatherId);
}