using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public TeamType Type { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Epic> Epics { get; set; } = new List<Epic>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}

public class User
{
    public int Id { get; set; }
    public required string IdentityId { get; set; }
    public required string DisplayName { get; set; }
    public required string Email { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public required string Role { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? JoinDate { get; set; }
    public bool OptOutCelebrations { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Skills { get; set; }
    public string? Squad { get; set; }
    public int TotalPoints { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Activity> OwnedActivities { get; set; } = new List<Activity>();
    public ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public ICollection<PointsTransaction> PointsTransactions { get; set; } = new List<PointsTransaction>();
    public ICollection<Kudos> KudosSent { get; set; } = new List<Kudos>();
    public ICollection<Kudos> KudosReceived { get; set; } = new List<Kudos>();
}

public class Epic
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public EpicStatus Status { get; set; } = EpicStatus.Backlog;
    public string? JiraKey { get; set; }
    public int? AdoWorkItemId { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}

public class Activity
{
    public int Id { get; set; }
    public int EpicId { get; set; }
    public Epic Epic { get; set; } = null!;
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public required string Title { get; set; }
    public string? Description { get; set; }
    public EpicStatus Status { get; set; } = EpicStatus.Backlog;
    public int? OwnerUserId { get; set; }
    public User? Owner { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? DueAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Tags { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ActivityKpiValue> KpiValues { get; set; } = new List<ActivityKpiValue>();
}

public class KPI
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public KpiCategory Category { get; set; }
    public UnitType UnitType { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<ActivityKpiValue> ActivityValues { get; set; } = new List<ActivityKpiValue>();
}

public class ActivityKpiValue
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
    public int KpiId { get; set; }
    public KPI Kpi { get; set; } = null!;
    public decimal? NumericValue { get; set; }
    public bool? BoolValue { get; set; }
    public int? DurationSeconds { get; set; }
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
}

public class Deployment
{
    public int Id { get; set; }
    public Enums.Environment Env { get; set; }
    public DateTime PlannedAt { get; set; }
    public DateTime? DeployedAt { get; set; }
    public DeploymentResult? Result { get; set; }
    public string? Notes { get; set; }
    public string? ReleaseId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class RegressionRun
{
    public int Id { get; set; }
    public Enums.Environment Env { get; set; } = Enums.Environment.Dev;
    public DateTime RunAt { get; set; }
    public int Total { get; set; }
    public int Passed { get; set; }
    public int Failed { get; set; }
    public int DurationSeconds { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PipelineRun
{
    public int Id { get; set; }
    public Enums.Environment Env { get; set; }
    public required string PipelineName { get; set; }
    public required string RunId { get; set; }
    public PipelineStatus Status { get; set; }
    public int DurationSeconds { get; set; }
    public DateTime QueuedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool IsOptimizedFlag { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CostTarget
{
    public int Id { get; set; }
    public int Year { get; set; }
    public ScopeType ScopeType { get; set; }
    public required string ScopeId { get; set; }
    public decimal MonthlyTarget { get; set; }
    public decimal AnnualTarget { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CostActual
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public required string ScopeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CodeQualitySnapshot
{
    public int Id { get; set; }
    public CodeQualitySource Source { get; set; }
    public required string ProjectKey { get; set; }
    public DateTime SnapshotWeekStart { get; set; }
    public int Bugs { get; set; }
    public int Vulns { get; set; }
    public int Smells { get; set; }
    public decimal? CoveragePct { get; set; }
    public int Criticals { get; set; }
    public int Highs { get; set; }
    public int Mediums { get; set; }
    public int Lows { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class DbaMaintenance
{
    public int Id { get; set; }
    public DbEnvironment Env { get; set; }
    public required string Instance { get; set; }
    public required string Database { get; set; }
    public required string TaskType { get; set; }
    public DateTime PlannedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Notes { get; set; }
    public string? Impact { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LearningContent
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public required string MarkdownContent { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Quiz
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public int PassingScore { get; set; }
    public int TimeLimitSeconds { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    public ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
}

public class QuizQuestion
{
    public int Id { get; set; }
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public required string Text { get; set; }
    public QuizQuestionType Type { get; set; }
    public required string ChoicesJson { get; set; }
    public required string CorrectAnswersJson { get; set; }
    public int Points { get; set; }
    public int OrderIndex { get; set; }
}

public class QuizAttempt
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int Score { get; set; }
    public bool Passed { get; set; }
    public string? AnswersJson { get; set; }
}

public class Certificate
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public required string Title { get; set; }
    public required string Issuer { get; set; }
    public DateTime EarnedAt { get; set; }
    public string? Url { get; set; }
}

public class PointsTransaction
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int Delta { get; set; }
    public required string Reason { get; set; }
    public required string Source { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Kudos
{
    public int Id { get; set; }
    public int FromUserId { get; set; }
    public User FromUser { get; set; } = null!;
    public int ToUserId { get; set; }
    public User ToUser { get; set; } = null!;
    public required string Message { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? ApprovedByUserId { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

public class Milestone
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; set; }
    public MilestoneType Type { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Newsletter
{
    public int Id { get; set; }
    public NewsletterPeriod Period { get; set; }
    public DateTime RangeStart { get; set; }
    public DateTime RangeEnd { get; set; }
    public required string Markdown { get; set; }
    public required string Html { get; set; }
    public DateTime? PublishedAt { get; set; }
    public int? PublishedByUserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class IntegrationCredential
{
    public int Id { get; set; }
    public IntegrationType Type { get; set; }
    public required string DisplayName { get; set; }
    public required string EncryptedPayloadJson { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class NotificationTemplate
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public NotificationChannel Channel { get; set; }
    public required string TemplateContent { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class AuditLog
{
    public int Id { get; set; }
    public int? ActorUserId { get; set; }
    public required string Action { get; set; }
    public required string Entity { get; set; }
    public string? EntityId { get; set; }
    public string? DataJson { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
