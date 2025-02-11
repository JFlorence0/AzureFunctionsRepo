using Microsoft.EntityFrameworkCore;
using http.Models;

namespace http.context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define DbSets for your tables
        public DbSet<InvestmentTheme> InvestmentThemes { get; set; }
        public DbSet<InvestmentIdea> InvestmentIdeas { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map table names
            modelBuilder.Entity<InvestmentTheme>().ToTable("investment_themes");
            modelBuilder.Entity<InvestmentIdea>().ToTable("investment_ideas");
            modelBuilder.Entity<Course>().ToTable("courses");

            // Define relationships
            modelBuilder.Entity<InvestmentIdea>()
                .HasOne(i => i.InvestmentTheme)
                .WithMany(t => t.InvestmentIdeas)
                .HasForeignKey(i => i.InvestmentThemeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
