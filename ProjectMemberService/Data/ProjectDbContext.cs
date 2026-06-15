using Microsoft.EntityFrameworkCore;
using ProjectMemberService.Models;

namespace ProjectMemberService.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
        public DbSet<Sprint> Sprints => Set<Sprint>();
        public DbSet<Milestone> Milestones => Set<Milestone>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Color).HasMaxLength(7);
                entity.Property(e => e.CreatedBy).IsRequired();
                entity.Property(e => e.Status).HasConversion<string>();
            });

            // ProjectMember
            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(200);
                entity.Property(e => e.Email).HasMaxLength(300);
                entity.Property(e => e.Role).HasConversion<string>();

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Members)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Mỗi user chỉ được thêm 1 lần vào project
                entity.HasIndex(e => new { e.ProjectId, e.UserId }).IsUnique();
            });

            // Sprint
            modelBuilder.Entity<Sprint>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Goal).HasMaxLength(1000);
                entity.Property(e => e.Status).HasConversion<string>();

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Sprints)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Milestone
            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                entity.HasOne(e => e.Project)
                      .WithMany(p => p.Milestones)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== SEED DATA FOR DEMO =====
            var demoProjectId = new Guid("11111111-1111-1111-1111-111111111111");

            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id = demoProjectId,
                Name = "Dự án Demo Phân Quyền",
                Description = "Dự án mẫu để chạy thử nghiệm các cấp bậc phân quyền (Owner, Manager, Member, Viewer, Admin)",
                StartDate = new DateTime(2026, 1, 1),
                EndDate = new DateTime(2026, 12, 31),
                Color = "#4F46E5",
                Status = ProjectStatus.Active,
                CreatedBy = "owner-demo",
                CreatedAt = new DateTime(2026, 1, 1),
                UpdatedAt = new DateTime(2026, 1, 1)
            });

            modelBuilder.Entity<ProjectMember>().HasData(
                new ProjectMember
                {
                    Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    ProjectId = demoProjectId,
                    UserId = "owner-demo",
                    DisplayName = "Demo Project Owner",
                    Email = "owner@demo.com",
                    Role = MemberRole.Owner,
                    JoinedAt = new DateTime(2026, 1, 1)
                },
                new ProjectMember
                {
                    Id = new Guid("33333333-3333-3333-3333-333333333333"),
                    ProjectId = demoProjectId,
                    UserId = "manager-demo",
                    DisplayName = "Demo Project Manager",
                    Email = "manager@demo.com",
                    Role = MemberRole.Manager,
                    JoinedAt = new DateTime(2026, 1, 1)
                },
                new ProjectMember
                {
                    Id = new Guid("44444444-4444-4444-4444-444444444444"),
                    ProjectId = demoProjectId,
                    UserId = "member-demo",
                    DisplayName = "Demo Project Member",
                    Email = "member@demo.com",
                    Role = MemberRole.Member,
                    JoinedAt = new DateTime(2026, 1, 1)
                },
                new ProjectMember
                {
                    Id = new Guid("55555555-5555-5555-5555-555555555555"),
                    ProjectId = demoProjectId,
                    UserId = "viewer-demo",
                    DisplayName = "Demo Project Viewer",
                    Email = "viewer@demo.com",
                    Role = MemberRole.Viewer,
                    JoinedAt = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
