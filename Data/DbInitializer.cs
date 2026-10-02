using DotNetApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetApp.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Projects.AnyAsync())
        {
            return; // DB has been seeded
        }

        var sampleProjects = new List<ProjectItem>
        {
            new()
            {
                Title = "Cloud Infrastructure Modernization",
                Description = "Migrate legacy microservices to Azure Container Apps with distributed telemetry and autoscaling.",
                Category = ProjectCategory.DevOps,
                Priority = ProjectPriority.Critical,
                Status = ProjectStatus.InProgress,
                Assignee = "Alex Rivera",
                ProgressPercentage = 65,
                EstimatedHours = 80,
                LoggedHours = 52,
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                Tags = "azure,k8s,devops,terraform",
                IsStarred = true
            },
            new()
            {
                Title = "AI-Powered Customer Insights Engine",
                Description = "Build LLM-assisted semantic search and automated sentiment analyzer for support interactions.",
                Category = ProjectCategory.AI,
                Priority = ProjectPriority.High,
                Status = ProjectStatus.InProgress,
                Assignee = "Elena Rostov",
                ProgressPercentage = 40,
                EstimatedHours = 120,
                LoggedHours = 48,
                DueDate = DateTime.UtcNow.AddDays(21),
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                Tags = "ai,genai,embeddings,python,csharp",
                IsStarred = true
            },
            new()
            {
                Title = "Real-time Analytics Dashboard UI",
                Description = "Design and implement high-performance interactive charts with WebSockets and Blazor Server.",
                Category = ProjectCategory.Frontend,
                Priority = ProjectPriority.High,
                Status = ProjectStatus.InReview,
                Assignee = "Marcus Vance",
                ProgressPercentage = 90,
                EstimatedHours = 60,
                LoggedHours = 54,
                DueDate = DateTime.UtcNow.AddDays(3),
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                Tags = "blazor,charts,ui,realtime",
                IsStarred = false
            },
            new()
            {
                Title = "High-Throughput Payment Gateway V2",
                Description = "Re-architect payment checkout pipeline to support idempotent webhook retries and PCI-DSS compliance.",
                Category = ProjectCategory.Backend,
                Priority = ProjectPriority.Critical,
                Status = ProjectStatus.InProgress,
                Assignee = "Sophia Chen",
                ProgressPercentage = 55,
                EstimatedHours = 140,
                LoggedHours = 77,
                DueDate = DateTime.UtcNow.AddDays(14),
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                Tags = "payments,security,dotnet,efcore",
                IsStarred = true
            },
            new()
            {
                Title = "Mobile Companion App Design System",
                Description = "Create Figma component library and cross-platform token system for .NET MAUI / iOS / Android.",
                Category = ProjectCategory.Design,
                Priority = ProjectPriority.Medium,
                Status = ProjectStatus.Completed,
                Assignee = "Liam Gallagher",
                ProgressPercentage = 100,
                EstimatedHours = 45,
                LoggedHours = 45,
                DueDate = DateTime.UtcNow.AddDays(-2),
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                Tags = "design,figma,tokens,maui",
                IsStarred = false
            },
            new()
            {
                Title = "Zero-Trust Identity & RBAC Overhaul",
                Description = "Implement OpenID Connect, OAuth2 PKCE flow, and granular role/policy authorization checks.",
                Category = ProjectCategory.Backend,
                Priority = ProjectPriority.High,
                Status = ProjectStatus.Backlog,
                Assignee = "David Kim",
                ProgressPercentage = 10,
                EstimatedHours = 90,
                LoggedHours = 9,
                DueDate = DateTime.UtcNow.AddDays(30),
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Tags = "auth,oauth2,security,jwt",
                IsStarred = false
            },
            new()
            {
                Title = "GraphQL API Gateway & Schema Federation",
                Description = "Unify 6 microservices under a federated GraphQL gateway with query caching and rate limiting.",
                Category = ProjectCategory.Fullstack,
                Priority = ProjectPriority.Medium,
                Status = ProjectStatus.Backlog,
                Assignee = "Zara Morales",
                ProgressPercentage = 5,
                EstimatedHours = 110,
                LoggedHours = 5,
                DueDate = DateTime.UtcNow.AddDays(45),
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Tags = "graphql,api,caching,dotnet8",
                IsStarred = false
            }
        };

        await context.Projects.AddRangeAsync(sampleProjects);

        var sampleLogs = new List<ActivityLog>
        {
            new()
            {
                Action = "Created",
                EntityTitle = "GraphQL API Gateway & Schema Federation",
                Details = "Created project with target net8.0 backend",
                IconType = "add",
                Timestamp = DateTime.UtcNow.AddHours(-3)
            },
            new()
            {
                Action = "Status Changed",
                EntityTitle = "Real-time Analytics Dashboard UI",
                Details = "Moved status to InReview for design QA",
                IconType = "check",
                Timestamp = DateTime.UtcNow.AddHours(-14)
            },
            new()
            {
                Action = "Progress Updated",
                EntityTitle = "Cloud Infrastructure Modernization",
                Details = "Progress increased to 65% (Terraform modules completed)",
                IconType = "edit",
                Timestamp = DateTime.UtcNow.AddDays(-1)
            },
            new()
            {
                Action = "Completed",
                EntityTitle = "Mobile Companion App Design System",
                Details = "Design tokens exported and reviewed by team",
                IconType = "check",
                Timestamp = DateTime.UtcNow.AddDays(-2)
            }
        };

        await context.ActivityLogs.AddRangeAsync(sampleLogs);
        await context.SaveChangesAsync();
    }
}
