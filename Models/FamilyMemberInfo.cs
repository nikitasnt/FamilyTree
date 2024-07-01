namespace FamilyTree.Models;

/// <summary>
/// Output data about a family member.
/// </summary>
public class FamilyMemberInfo
{
    public int Id { get; set; }
    
    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Birthday { get; set; }
}