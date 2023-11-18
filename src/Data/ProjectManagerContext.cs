using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;

namespace ProjectManager.Data
{
    public class ProjectManagerContext : DbContext
    {
        public DbSet<Staff> Staffs { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public ProjectManagerContext (DbContextOptions<ProjectManagerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>()
                .HasMany(e => e.ProjectsManager)
                .WithOne(e => e.Manager)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Staff>()
                .HasMany(c => c.ProjectsEmployee)
                .WithMany(p => p.Employees)
                .UsingEntity(
                    "ProjectStaff",
                    l => l.HasOne(typeof(Project)).WithMany().HasForeignKey("ProjectsId").HasPrincipalKey(nameof(Project.Id)),
                    r => r.HasOne(typeof(Staff)).WithMany().HasForeignKey("StaffsId").HasPrincipalKey(nameof(Staff.Id)),
                    j => j.HasKey("StaffsId", "ProjectsId"));
        }
    }
}
