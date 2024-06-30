namespace FamilyTree.Models;

public class FamilyMemberWithChildrenInfo
{
    public int Id { get; set; }
    
    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Birthday { get; set; }
    
    public IEnumerable<FamilyMemberWithChildrenInfo> DirectChildren { get; set; } = null!;
}