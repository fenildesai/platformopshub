using System;
using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Application.DTOs;

public class EpicDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string TeamName { get; set; } = null!;
    public EpicStatus Status { get; set; }
    public string? JiraKey { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ActivityDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string TeamName { get; set; } = null!;
    public EpicStatus Status { get; set; }
    public string? OwnerName { get; set; }
    public DateTime? DueAt { get; set; }
    public string? Tags { get; set; }
}

public class CodeQualitySummaryDto
{
    public string ProjectKey { get; set; } = null!;
    public int Bugs { get; set; }
    public int Vulnerabilities { get; set; }
    public int CodeSmells { get; set; }
    public double CoveragePct { get; set; }
    public string Status { get; set; } = null!;
}

public class DbaMaintenanceDto
{
    public int Id { get; set; }
    public string Instance { get; set; } = null!;
    public string Database { get; set; } = null!;
    public string TaskType { get; set; } = null!;
    public DateTime PlannedAt { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted => CompletedAt.HasValue;
    public DateTime? CompletedAt { get; set; }
}

public class UserSummaryDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? Squad { get; set; }
    public string? Skills { get; set; }
    public string? PhotoUrl { get; set; }
}

public class TeamSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<UserSummaryDto> Members { get; set; } = new();
}

public class LeaderboardEntryDto
{
    public int UserId { get; set; }
    public string DisplayName { get; set; } = null!;
    public string? PhotoUrl { get; set; }
    public int Points { get; set; }
    public int KudosReceivedCount { get; set; }
    public string? TeamName { get; set; }
    public int DailyStreak { get; set; }
    public List<string> RecentAchievements { get; set; } = new();
}

public class EpicDetailDto : EpicDto
{
    public string? CreatedBy { get; set; }
    public List<ActivityDto> Activities { get; set; } = new();
}

public class ActivityDetailDto : ActivityDto
{
    public string? LongDescription { get; set; }
    public List<string> UpdateLogs { get; set; } = new();
}

public class CostOptimizationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal EstimatedSavings { get; set; }
    public string Status { get; set; } = null!;
    public string? OwnerName { get; set; }
}
