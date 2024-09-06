using JobHunterAPI.Middleware;
using JobHunterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JobHunterAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }

        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<Category> categories { get; set; } 
        public DbSet<Job> jobs { get;   set;}
        public DbSet<JobCategory> JobCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobCategory>()
                .HasKey(jc => new { jc.JobId, jc.CategoryId });

            modelBuilder.Entity<JobCategory>()
                .HasOne(jc => jc.Job)
                .WithMany(j => j.JobCategories)
                .HasForeignKey(jc => jc.JobId);

            modelBuilder.Entity<JobCategory>()
                .HasOne(jc => jc.Category)
                .WithMany(c => c.JobCategories)
                .HasForeignKey(jc => jc.CategoryId);
        }
    }
}
