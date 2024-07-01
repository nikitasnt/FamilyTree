using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Database.Models;

/// <summary>
/// Information about a family member in a family tree.
/// </summary>
[Index(nameof(HierarchyPath))]
public class FamilyMember
{
    /// <summary>
    /// Family member ID. The database is generated.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    /// <summary>
    /// Firstname.
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string Firstname { get; set; } = null!;
    
    /// <summary>
    /// Lastname.
    /// </summary>
    [Required]
    [MaxLength(60)]
    public string Lastname { get; set; } = null!;

    /// <summary>
    /// Birthday date.
    /// </summary>
    [Required]
    public DateOnly Birthday { get; set; }
    
    /// <summary>
    /// LTree consisting of <see cref="Id"/>. Operations allowed only for not materialized entity.
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