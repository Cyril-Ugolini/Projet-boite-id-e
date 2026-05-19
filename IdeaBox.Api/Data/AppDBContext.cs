using Microsoft.EntityFrameworkCore;
using IdeaBox.Api.Models;

namespace IdeaBox.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Idee> Idees { get; set; }
    public DbSet<Commentaire> Commentaires { get; set; }
    public DbSet<Vote> Votes { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Contraintes CHECK priorite et difficulte
    modelBuilder.Entity<Idee>().ToTable(t =>
    {
        t.HasCheckConstraint("CK_idee_priorite",   "priorite IN ('basse', 'moyenne', 'haute')");
        t.HasCheckConstraint("CK_idee_difficulte", "difficulte IN ('basse', 'moyenne', 'haute')");
    });

    // Contrainte UNIQUE vote (Id_idee + auteur)
    modelBuilder.Entity<Vote>()
        .HasIndex(v => new { v.IdIdee, v.Auteur })
        .IsUnique();

    // Cascade delete
    modelBuilder.Entity<Commentaire>()
        .HasOne(c => c.Idee)
        .WithMany(i => i.Commentaires)
        .HasForeignKey(c => c.IdIdee)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Vote>()
        .HasOne(v => v.Idee)
        .WithMany(i => i.Votes)
        .HasForeignKey(v => v.IdIdee)
        .OnDelete(DeleteBehavior.Cascade);
}
}