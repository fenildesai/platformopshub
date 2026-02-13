using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlatformOpsHub.Domain.Entities;
using PlatformOpsHub.Domain.Enums;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Infrastructure.Seed;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public DatabaseSeeder(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        // Seed roles
        await SeedRolesAsync();

        // Seed admin user
        await SeedAdminUserAsync();

        // Seed demo data if configured
        var seedDemo = _configuration.GetValue<bool>("SEED_DEMO");
        if (seedDemo && !await _context.Teams.AnyAsync())
        {
            await SeedDemoDataAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        string[] roles = { "Admin", "Manager", "Member", "Viewer" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private async Task SeedAdminUserAsync()
    {
        var adminEmail = _configuration["ADMIN_EMAIL"] ?? "admin@platformops.local";
        var adminPwd = _configuration["ADMIN_PWD"] ?? "Admin@123456";

        var existingAdmin = await _userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(adminUser, adminPwd);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");

                // Create User entity
                var user = new User
                {
                    IdentityId = adminUser.Id,
                    DisplayName = "Platform Admin",
                    Email = adminEmail,
                    Role = "Admin",
                    JoinDate = DateTime.UtcNow
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task SeedDemoDataAsync()
    {
        // Seed Teams
        var serviceTeam = new Team { Name = "Service Team", Type = TeamType.Service, Description = "Manages service operations and deployments" };
        var changeTeam = new Team { Name = "Change Team", Type = TeamType.Change, Description = "Drives platform improvements and cost optimization" };
        var dbaTeam = new Team { Name = "DBA Team", Type = TeamType.DBA, Description = "Database administration and optimization" };

        _context.Teams.AddRange(serviceTeam, changeTeam, dbaTeam);
        await _context.SaveChangesAsync();

        // Seed KPIs
        var kpis = new List<KPI>
        {
            new() { Name = "Security Vulnerabilities Fixed", Category = KpiCategory.SecurityAndCompliance, UnitType = UnitType.Number, Description = "Number of security vulnerabilities remediated" },
            new() { Name = "Code Coverage", Category = KpiCategory.SecurityAndCompliance, UnitType = UnitType.Percentage, Description = "Percentage of code covered by tests" },
            new() { Name = "Average Build Time", Category = KpiCategory.AverageBuildTime, UnitType = UnitType.Duration, Description = "Average pipeline build duration" },
            new() { Name = "Time to Release", Category = KpiCategory.TimeToRelease, UnitType = UnitType.Duration, Description = "Time from commit to production" },
            new() { Name = "Mean Time To Recover", Category = KpiCategory.MTTR, UnitType = UnitType.Duration, Description = "Average time to recover from incidents" },
            new() { Name = "Change Failure Rate", Category = KpiCategory.ChangeFailureRate, UnitType = UnitType.Percentage, Description = "Percentage of changes causing failures" },
            new() { Name = "Cost Savings Achieved", Category = KpiCategory.CostOptimisation, UnitType = UnitType.Number, Description = "Cost savings in GBP" },
            new() { Name = "GenAI Features Delivered", Category = KpiCategory.GenAIInitiatives, UnitType = UnitType.Number, Description = "Number of GenAI features shipped" },
            new() { Name = "Pipeline Optimization", Category = KpiCategory.PerformanceAndEfficiency, UnitType = UnitType.Boolean, Description = "Pipeline optimized yes/no" },
            new() { Name = "Team Training Completed", Category = KpiCategory.Growth, UnitType = UnitType.Boolean, Description = "Training completed yes/no" }
        };

        _context.KPIs.AddRange(kpis);
        await _context.SaveChangesAsync();

        // Seed Epics
        var epic1 = new Epic { Title = "Nightly Regression Stability", Description = "Improve nightly regression pass rate to 95%", TeamId = serviceTeam.Id, Status = EpicStatus.InProgress, CreatedBy = "Platform Admin" };
        var epic2 = new Epic { Title = "2026 Cost Optimization", Description = "Achieve £350K annual cost target", TeamId = changeTeam.Id, Status = EpicStatus.InProgress, CreatedBy = "Platform Admin" };
        var epic3 = new Epic { Title = "Database Performance Tuning", Description = "Optimize top 10 slow queries", TeamId = dbaTeam.Id, Status = EpicStatus.Backlog, CreatedBy = "Platform Admin" };

        _context.Epics.AddRange(epic1, epic2, epic3);
        await _context.SaveChangesAsync();

        // Seed Activities
        var activities = new List<Activity>
        {
            new() { Title = "Fix flaky UI tests", Description = "Stabilize Selenium tests", EpicId = epic1.Id, TeamId = serviceTeam.Id, Status = EpicStatus.InProgress, StartAt = DateTime.UtcNow.AddDays(-7) },
            new() { Title = "Implement retry logic", Description = "Add retry for transient failures", EpicId = epic1.Id, TeamId = serviceTeam.Id, Status = EpicStatus.Done, StartAt = DateTime.UtcNow.AddDays(-14), CompletedAt = DateTime.UtcNow.AddDays(-2) },
            new() { Title = "Right-size Azure VMs", Description = "Downsize over-provisioned VMs", EpicId = epic2.Id, TeamId = changeTeam.Id, Status = EpicStatus.InProgress, StartAt = DateTime.UtcNow.AddDays(-10) },
            new() { Title = "Implement auto-shutdown", Description = "Auto-shutdown non-prod resources", EpicId = epic2.Id, TeamId = changeTeam.Id, Status = EpicStatus.Done, StartAt = DateTime.UtcNow.AddDays(-20), CompletedAt = DateTime.UtcNow.AddDays(-5) },
            new() { Title = "Index optimization", Description = "Rebuild fragmented indexes", EpicId = epic3.Id, TeamId = dbaTeam.Id, Status = EpicStatus.Backlog }
        };

        _context.Activities.AddRange(activities);
        await _context.SaveChangesAsync();

        // Seed Activity KPI Values
        var kpiValues = new List<ActivityKpiValue>
        {
            new() { ActivityId = activities[1].Id, KpiId = kpis[8].Id, BoolValue = true, CapturedAt = DateTime.UtcNow.AddDays(-2) },
            new() { ActivityId = activities[3].Id, KpiId = kpis[6].Id, NumericValue = 15000, CapturedAt = DateTime.UtcNow.AddDays(-5) }
        };

        _context.ActivityKpiValues.AddRange(kpiValues);

        // Seed 2026 Cost Target
        var costTarget = new CostTarget
        {
            Year = 2026,
            ScopeType = ScopeType.Subscription,
            ScopeId = "prod-subscription-001",
            MonthlyTarget = 29166.67m, // £350K / 12
            AnnualTarget = 350000m
        };

        _context.CostTargets.Add(costTarget);

        // Seed Cost Actuals (YTD)
        var costActuals = new List<CostActual>();
        for (int i = 1; i <= DateTime.UtcNow.Month; i++)
        {
            costActuals.Add(new CostActual
            {
                Date = new DateTime(2026, i, 1),
                ScopeId = "prod-subscription-001",
                Amount = 27500m + (decimal)(new Random().NextDouble() * 3000) // Slightly under target
            });
        }

        _context.CostActuals.AddRange(costActuals);

        // Seed Code Quality Snapshots
        var weekStart = DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek).Date;
        var snapshots = new List<CodeQualitySnapshot>
        {
            new() { Source = CodeQualitySource.SonarQube, ProjectKey = "platform-api", SnapshotWeekStart = weekStart.AddDays(-14), Bugs = 15, Vulns = 8, Smells = 120, CoveragePct = 72.5m, Criticals = 2, Highs = 6, Mediums = 10, Lows = 15 },
            new() { Source = CodeQualitySource.SonarQube, ProjectKey = "platform-api", SnapshotWeekStart = weekStart.AddDays(-7), Bugs = 12, Vulns = 6, Smells = 110, CoveragePct = 74.2m, Criticals = 1, Highs = 5, Mediums = 8, Lows = 12 },
            new() { Source = CodeQualitySource.SonarQube, ProjectKey = "platform-api", SnapshotWeekStart = weekStart, Bugs = 10, Vulns = 4, Smells = 105, CoveragePct = 76.8m, Criticals = 0, Highs = 4, Mediums = 6, Lows = 10 },
            new() { Source = CodeQualitySource.Checkmarx, ProjectKey = "platform-web", SnapshotWeekStart = weekStart.AddDays(-14), Bugs = 0, Vulns = 22, Smells = 0, Criticals = 5, Highs = 10, Mediums = 7, Lows = 0 },
            new() { Source = CodeQualitySource.Checkmarx, ProjectKey = "platform-web", SnapshotWeekStart = weekStart.AddDays(-7), Bugs = 0, Vulns = 18, Smells = 0, Criticals = 3, Highs = 8, Mediums = 7, Lows = 0 },
            new() { Source = CodeQualitySource.Checkmarx, ProjectKey = "platform-web", SnapshotWeekStart = weekStart, Bugs = 0, Vulns = 14, Smells = 0, Criticals = 2, Highs = 6, Mediums = 6, Lows = 0 }
        };

        _context.CodeQualitySnapshots.AddRange(snapshots);

        // Seed Learning Content
        var learningContents = new List<LearningContent>
        {
            new() { Title = "Azure DevOps 101", Category = "ADO101", MarkdownContent = "# Azure DevOps 101\n\n## Introduction\nAzure DevOps is a comprehensive suite of development tools...\n\n## Key Features\n- Repos: Git repositories\n- Pipelines: CI/CD automation\n- Boards: Work tracking\n- Test Plans: Quality assurance\n- Artifacts: Package management", IsPublished = true },
            new() { Title = "Azure 101", Category = "Azure101", MarkdownContent = "# Azure 101\n\n## Cloud Computing Basics\nMicrosoft Azure is a cloud computing platform...\n\n## Core Services\n- Compute: VMs, App Services, Functions\n- Storage: Blob, Files, Queues\n- Databases: SQL, Cosmos DB\n- Networking: VNets, Load Balancers\n- Identity: Azure AD", IsPublished = true },
            new() { Title = "GenAI 101", Category = "GenAI101", MarkdownContent = "# GenAI 101\n\n## Generative AI Overview\nGenerative AI creates new content from learned patterns...\n\n## Azure OpenAI Service\n- GPT models for text generation\n- DALL-E for image creation\n- Embeddings for semantic search\n- Responsible AI practices\n- Use cases: chatbots, summarization, code generation", IsPublished = true }
        };

        _context.LearningContents.AddRange(learningContents);

        // Seed Quiz
        var quiz = new Quiz
        {
            Title = "Azure DevOps Fundamentals",
            Category = "ADO101",
            PassingScore = 70,
            TimeLimitSeconds = 600
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();

        var questions = new List<QuizQuestion>
        {
            new() { QuizId = quiz.Id, Text = "What is Azure Pipelines used for?", Type = QuizQuestionType.SingleChoice, ChoicesJson = "[\"CI/CD automation\",\"Version control\",\"Work tracking\",\"Package management\"]", CorrectAnswersJson = "[0]", Points = 10, OrderIndex = 1 },
            new() { QuizId = quiz.Id, Text = "Which Azure DevOps service provides Git repositories?", Type = QuizQuestionType.SingleChoice, ChoicesJson = "[\"Boards\",\"Repos\",\"Pipelines\",\"Artifacts\"]", CorrectAnswersJson = "[1]", Points = 10, OrderIndex = 2 },
            new() { QuizId = quiz.Id, Text = "Select all valid pipeline triggers:", Type = QuizQuestionType.MultipleChoice, ChoicesJson = "[\"Push\",\"Pull Request\",\"Scheduled\",\"Manual\"]", CorrectAnswersJson = "[0,1,2,3]", Points = 15, OrderIndex = 3 }
        };

        _context.QuizQuestions.AddRange(questions);

        // Seed Deployments
        var deployments = new List<Deployment>
        {
            new() { Env = Domain.Enums.Environment.Dev, PlannedAt = DateTime.UtcNow.AddDays(-7), DeployedAt = DateTime.UtcNow.AddDays(-7), Result = DeploymentResult.Success, ReleaseId = "R-2026-001" },
            new() { Env = Domain.Enums.Environment.Staging, PlannedAt = DateTime.UtcNow.AddDays(-5), DeployedAt = DateTime.UtcNow.AddDays(-5), Result = DeploymentResult.Success, ReleaseId = "R-2026-001" },
            new() { Env = Domain.Enums.Environment.Prod, PlannedAt = DateTime.UtcNow.AddDays(-3), DeployedAt = DateTime.UtcNow.AddDays(-3), Result = DeploymentResult.Success, ReleaseId = "R-2026-001" },
            new() { Env = Domain.Enums.Environment.Dev, PlannedAt = DateTime.UtcNow.AddDays(-2), DeployedAt = DateTime.UtcNow.AddDays(-2), Result = DeploymentResult.Fail, ReleaseId = "R-2026-002", Notes = "Database migration failed" },
            new() { Env = Domain.Enums.Environment.Prod, PlannedAt = DateTime.UtcNow.AddDays(2), Result = null, ReleaseId = "R-2026-003" }
        };

        _context.Deployments.AddRange(deployments);

        // Seed Regression Runs
        var regressionRuns = new List<RegressionRun>();
        for (int i = 0; i < 14; i++)
        {
            var passRate = 0.85 + (new Random().NextDouble() * 0.12); // 85-97%
            var total = 250;
            var passed = (int)(total * passRate);
            regressionRuns.Add(new RegressionRun
            {
                Env = Domain.Enums.Environment.Dev,
                RunAt = DateTime.UtcNow.AddDays(-i).Date.AddHours(2),
                Total = total,
                Passed = passed,
                Failed = total - passed,
                DurationSeconds = 1800 + new Random().Next(-300, 300)
            });
        }

        _context.RegressionRuns.AddRange(regressionRuns);

        // Seed Notification Templates
        var templates = new List<NotificationTemplate>
        {
            new() { Key = "Birthday", Channel = NotificationChannel.Teams, TemplateContent = "{\"type\":\"AdaptiveCard\",\"body\":[{\"type\":\"TextBlock\",\"text\":\"🎉 Happy Birthday {{DisplayName}}!\",\"size\":\"Large\",\"weight\":\"Bolder\"}]}", IsActive = true },
            new() { Key = "Anniversary", Channel = NotificationChannel.Teams, TemplateContent = "{\"type\":\"AdaptiveCard\",\"body\":[{\"type\":\"TextBlock\",\"text\":\"🎊 Happy Work Anniversary {{DisplayName}}!\",\"size\":\"Large\",\"weight\":\"Bolder\"}]}", IsActive = true },
            new() { Key = "Newsletter", Channel = NotificationChannel.Teams, TemplateContent = "{\"type\":\"AdaptiveCard\",\"body\":[{\"type\":\"TextBlock\",\"text\":\"📰 New Newsletter Published\",\"size\":\"Large\",\"weight\":\"Bolder\"}]}", IsActive = true }
        };

        _context.NotificationTemplates.AddRange(templates);

        await _context.SaveChangesAsync();
    }
}
