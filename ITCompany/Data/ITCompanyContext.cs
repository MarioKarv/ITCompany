using ITCompany.Models;
using Microsoft.EntityFrameworkCore;

namespace ITCompany.Data
{
    public class ITCompanyContext : DbContext
    {
        public ITCompanyContext(DbContextOptions<ITCompanyContext> options) : base(options)
        {
        }

        public DbSet<User> Users{ get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects{ get; set; }
        public DbSet<ProjectType> ProjectTypes{ get; set; }
        public DbSet<ProjectCommand> ProjectCommands { get; set; }
        public DbSet<Task> Tasks { get; set; } 
        public DbSet<TaskCommand> TaskCommands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Project>().ToTable("Project").HasOne(x => x.Client).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ProjectType>().ToTable("ProjectType");
            modelBuilder.Entity<ProjectCommand>().ToTable("ProjectCommand").HasOne(x=>x.Employee).WithMany(x=>x.ProjectCommands).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Task>().ToTable("Task");
            modelBuilder.Entity<TaskCommand>().ToTable("TaskCommand").HasOne(x => x.Employee).WithMany(x => x.TaskCommands).OnDelete(DeleteBehavior.Restrict); ;
        }
    }
}
