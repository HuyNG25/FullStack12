using Microsoft.EntityFrameworkCore;
using ProjectMemberService.Models;

namespace ProjectMemberService.Data
{
    public static class DataSeeder
    {
        public static void Seed(ProjectDbContext context)
        {
            if (context.Projects.Any())
            {
                return;   // DB has been seeded
            }

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Dự án Mẫu N1 (Demo Phân Quyền)",
                Description = "Dự án chứa 21 người dùng mẫu để test phân quyền theo role",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(3),
                Status = ProjectStatus.Active,
                CreatedBy = "owner-user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Color = "#3B82F6"
            };

            context.Projects.Add(project);
            context.SaveChanges();

            // 1 Owner
            context.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = project.Id,
                UserId = "owner-user",
                DisplayName = "Owner N1",
                Email = "owner@n1.com",
                Role = MemberRole.Owner,
                JoinedAt = DateTime.UtcNow
            });

            // 5 Managers
            for (int i = 1; i <= 5; i++)
            {
                context.ProjectMembers.Add(new ProjectMember
                {
                    ProjectId = project.Id,
                    UserId = $"manager-user-{i}",
                    DisplayName = $"Manager {i}",
                    Email = $"manager{i}@n1.com",
                    Role = MemberRole.Manager,
                    JoinedAt = DateTime.UtcNow
                });
            }

            // 10 Members
            for (int i = 1; i <= 10; i++)
            {
                context.ProjectMembers.Add(new ProjectMember
                {
                    ProjectId = project.Id,
                    UserId = $"member-user-{i}",
                    DisplayName = $"Member {i}",
                    Email = $"member{i}@n1.com",
                    Role = MemberRole.Member,
                    JoinedAt = DateTime.UtcNow
                });
            }

            // 5 Viewers
            for (int i = 1; i <= 5; i++)
            {
                context.ProjectMembers.Add(new ProjectMember
                {
                    ProjectId = project.Id,
                    UserId = $"viewer-user-{i}",
                    DisplayName = $"Viewer {i}",
                    Email = $"viewer{i}@n1.com",
                    Role = MemberRole.Viewer,
                    JoinedAt = DateTime.UtcNow
                });
            }

            context.SaveChanges();
        }
    }
}
