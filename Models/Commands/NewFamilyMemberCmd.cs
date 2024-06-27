namespace FamilyTree.Models.Commands;

public class NewFamilyMemberCmd
{
    public required string Firstname { get; set; }
    
    public required string Lastname { get; set; }
    
    public required DateOnly Birthday { get; set; }
}