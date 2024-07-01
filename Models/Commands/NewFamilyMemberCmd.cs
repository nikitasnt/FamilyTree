namespace FamilyTree.Models.Commands;

/// <summary>
/// Data for creating a new family member.
/// </summary>
public class NewFamilyMemberCmd
{
    public required string Firstname { get; set; }
    
    public required string Lastname { get; set; }
    
    public required DateOnly Birthday { get; set; }
}