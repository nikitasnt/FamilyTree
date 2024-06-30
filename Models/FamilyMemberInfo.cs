﻿namespace FamilyTree.Models;

public class FamilyMemberInfo
{
    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public DateOnly Birthday { get; set; }
    
    public IEnumerable<FamilyMemberInfo> DirectChildren { get; set; } = null!;
}