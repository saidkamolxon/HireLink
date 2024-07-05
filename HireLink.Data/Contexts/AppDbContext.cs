using HireLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HireLink.Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Candidate> Candidates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var candidateBuilder = modelBuilder.Entity<Candidate>();
        modelBuilder.Entity<Candidate>()
            .HasIndex(c => c.Email)
                .IsUnique();

        #region Apply Case Insensitive Collation If Db is PostgreSQL
        /*
        if (Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
        {
            modelBuilder
                .HasCollation("case_insensitive_collation", locale: "en-u-ks-primary", provider: "icu", deterministic: false);

            modelBuilder.Entity<Candidate>()
                .Property(c => c.Email)
                .UseCollation("case_insensitive_collation");
        }
        */
        #endregion
    }
}
