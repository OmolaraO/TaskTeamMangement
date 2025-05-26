using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Models;

namespace TeamTaskManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }   

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }  
        public DbSet<TaskItem> Tasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeamUser>()
                .HasKey(tu => new { tu.TeamId, tu.UserId });

            modelBuilder.Entity<TeamUser>()
                .HasOne(tu => tu.Team)
                .WithMany(t => t.TeamUsers)
                .HasForeignKey(tu => tu.TeamId);

            modelBuilder.Entity<TeamUser>()
                .HasOne(tu => tu.User)
                .WithMany()
                .HasForeignKey(tu => tu.UserId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Team)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.TeamId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId);
        }



    }
}
