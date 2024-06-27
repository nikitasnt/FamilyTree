using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Database.Models;

[Index(nameof(HierarchyPath))]
public class FamilyMember
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string Firstname { get; set; } = null!;

    [Required]
    [MaxLength(60)]
    public string Lastname { get; set; } = null!;

    [Required]
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// LTree by IDs.
    /// </summary>
    [Required]
    public LTree HierarchyPath { get; set; }
}