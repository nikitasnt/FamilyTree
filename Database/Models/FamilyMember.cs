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
    /// LTree by IDs. Operations allowed only for not materialized entity.
    /// </summary>
    [Required]
    public LTree HierarchyPath { get; set; }
    
    /// <summary>
    /// <see cref="LTree.NLevel"/> from <see cref="HierarchyPath"/>.
    /// </summary>
    [NotMapped]
    public int HierarchyLevel { get; set; }
    
    /// <summary>
    /// Parent ID using <see cref="HierarchyPath"/>.
    /// </summary>
    [NotMapped]
    public int? ParentId { get; set; }
}