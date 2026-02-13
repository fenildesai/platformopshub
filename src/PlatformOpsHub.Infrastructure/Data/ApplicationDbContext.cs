using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Domain.Entities;

namespace PlatformOpsHub.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Teams => Set<Team>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Epic> Epics => Set<Epic>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<KPI> KPIs => Set<KPI>();
    public DbSet<ActivityKpiValue> ActivityKpiValues => Set<ActivityKpiValue>();
    public DbSet<Deployment> Deployments => Set<Deployment>();
    public DbSet<RegressionRun> RegressionRuns => Set<RegressionRun>();
    public DbSet<PipelineRun> PipelineRuns => Set<PipelineRun>();
    public DbSet<CostTarget> CostTargets => Set<CostTarget>();
    public DbSet<CostActual> CostActuals => Set<CostActual>();
    public DbSet<CodeQualitySnapshot> CodeQualitySnapshots => Set<CodeQualitySnapshot>();
    public DbSet<DbaMaintenance> DbaMaintenances => Set<DbaMaintenance>();
    public DbSet<LearningContent> LearningContents => Set<LearningContent>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
    public DbSet<Certificate> Certificates => Set<Certificate>();
    public DbSet<PointsTransaction> PointsTransactions => Set<PointsTransaction>();
    public DbSet<Kudos> Kudos => Set<Kudos>();
    public DbSet<Milestone> Milestones => Set<Milestone>();
    public DbSet<Newsletter> Newsletters => Set<Newsletter>();
    public DbSet<IntegrationCredential> IntegrationCredentials => Set<IntegrationCredential>();
    public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Team
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => e.Type);
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IdentityId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.IdentityId).IsUnique();
            entity.HasIndex(e => e.Email);
            entity.HasOne(e => e.Team).WithMany(t => t.Users).HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.SetNull);
        });

        // Epic
        modelBuilder.Entity<Epic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedBy).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.TeamId);
            entity.HasOne(e => e.Team).WithMany(t => t.Epics).HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.Restrict);
        });

        // Activity
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.TeamId);
            entity.HasIndex(e => e.EpicId);
            entity.HasOne(e => e.Epic).WithMany(ep => ep.Activities).HasForeignKey(e => e.EpicId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Team).WithMany(t => t.Activities).HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Owner).WithMany(u => u.OwnedActivities).HasForeignKey(e => e.OwnerUserId).OnDelete(DeleteBehavior.SetNull);
        });

        // KPI
        modelBuilder.Entity<KPI>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsActive);
        });

        // ActivityKpiValue
        modelBuilder.Entity<ActivityKpiValue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ActivityId);
            entity.HasIndex(e => e.KpiId);
            entity.HasOne(e => e.Activity).WithMany(a => a.KpiValues).HasForeignKey(e => e.ActivityId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Kpi).WithMany(k => k.ActivityValues).HasForeignKey(e => e.KpiId).OnDelete(DeleteBehavior.Restrict);
        });

        // Deployment
        modelBuilder.Entity<Deployment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Env);
            entity.HasIndex(e => e.PlannedAt);
        });

        // RegressionRun
        modelBuilder.Entity<RegressionRun>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.RunAt);
            entity.HasIndex(e => e.Env);
        });

        // PipelineRun
        modelBuilder.Entity<PipelineRun>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PipelineName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.RunId).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Env);
            entity.HasIndex(e => e.QueuedAt);
        });

        // CostTarget
        modelBuilder.Entity<CostTarget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ScopeId).IsRequired().HasMaxLength(200);
            entity.Property(e => e.MonthlyTarget).HasPrecision(18, 2);
            entity.Property(e => e.AnnualTarget).HasPrecision(18, 2);
            entity.HasIndex(e => new { e.Year, e.ScopeId });
        });

        // CostActual
        modelBuilder.Entity<CostActual>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ScopeId).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.HasIndex(e => new { e.Date, e.ScopeId });
        });

        // CodeQualitySnapshot
        modelBuilder.Entity<CodeQualitySnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ProjectKey).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CoveragePct).HasPrecision(5, 2);
            entity.HasIndex(e => new { e.Source, e.ProjectKey, e.SnapshotWeekStart });
        });

        // DbaMaintenance
        modelBuilder.Entity<DbaMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Instance).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Database).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TaskType).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Env);
            entity.HasIndex(e => e.PlannedAt);
        });

        // LearningContent
        modelBuilder.Entity<LearningContent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsPublished);
        });

        // Quiz
        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Category);
        });

        // QuizQuestion
        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).IsRequired();
            entity.Property(e => e.ChoicesJson).IsRequired();
            entity.Property(e => e.CorrectAnswersJson).IsRequired();
            entity.HasIndex(e => e.QuizId);
            entity.HasOne(e => e.Quiz).WithMany(q => q.Questions).HasForeignKey(e => e.QuizId).OnDelete(DeleteBehavior.Cascade);
        });

        // QuizAttempt
        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.QuizId);
            entity.HasIndex(e => e.StartedAt);
            entity.HasOne(e => e.User).WithMany(u => u.QuizAttempts).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Quiz).WithMany(q => q.Attempts).HasForeignKey(e => e.QuizId).OnDelete(DeleteBehavior.Restrict);
        });

        // Certificate
        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Issuer).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.UserId);
            entity.HasOne(e => e.User).WithMany(u => u.Certificates).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // PointsTransaction
        modelBuilder.Entity<PointsTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Source).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasOne(e => e.User).WithMany(u => u.PointsTransactions).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Kudos
        modelBuilder.Entity<Kudos>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
            entity.HasIndex(e => e.FromUserId);
            entity.HasIndex(e => e.ToUserId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasOne(e => e.FromUser).WithMany(u => u.KudosSent).HasForeignKey(e => e.FromUserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.ToUser).WithMany(u => u.KudosReceived).HasForeignKey(e => e.ToUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // Milestone
        modelBuilder.Entity<Milestone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Type);
        });

        // Newsletter
        modelBuilder.Entity<Newsletter>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Markdown).IsRequired();
            entity.Property(e => e.Html).IsRequired();
            entity.HasIndex(e => e.Period);
            entity.HasIndex(e => e.RangeStart);
        });

        // IntegrationCredential
        modelBuilder.Entity<IntegrationCredential>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.EncryptedPayloadJson).IsRequired();
            entity.HasIndex(e => e.Type);
        });

        // NotificationTemplate
        modelBuilder.Entity<NotificationTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TemplateContent).IsRequired();
            entity.HasIndex(e => new { e.Key, e.Channel }).IsUnique();
        });

        // AuditLog
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Entity).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.ActorUserId);
        });
    }
}
