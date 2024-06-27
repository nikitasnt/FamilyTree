using FamilyTree.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilyTree.Database;

public class FamilyTreeDbContext : DbContext
{
    public DbSet<FamilyMember> FamilyMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("ltree");
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<FamilyMember>()
            .HasIndex(fm => fm.HierarchyPath)
            .HasMethod("gist");
    }
}