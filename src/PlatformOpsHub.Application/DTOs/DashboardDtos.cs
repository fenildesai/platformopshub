using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Application.DTOs;

public record DashboardStatsDto
{
    // Epic Stats
    public int BacklogEpics { get; init; }
    public int InProgressEpics { get; init; }
    public int DoneEpics { get; init; }

    // Deployment Stats
    public int DeploymentsThisWeek { get; init; }
    public double NightlyRegressionPassRate { get; init; }

    // Pipeline Stats
    public double PipelineHealthPercentage { get; init; }

    // Cost Stats
    public decimal AnnualCostTarget { get; init; }
    public decimal CostYTD { get; init; }
    public decimal CostVariance { get; init; }

    // Code Quality Stats
    public int NewBugsCount { get; init; }
    public int NewVulnerabilitiesCount { get; init; }
    public double AverageCodeCoverage { get; init; }

    // DORA Metrics
    public double MeanTimeToRecoverHours { get; init; }
    public double LeadTimeDays { get; init; }
    public double ChangeFailureRatePercentage { get; init; }

    // Team Activities
    public int GenAIInitiativesCount { get; init; }
    public int DbaMaintenanceBacklogCount { get; init; }
}

public record EpicDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string TeamName { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

public record ActivityDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string OwnerName { get; init; } = string.Empty;
    public DateTime? DueAt { get; init; }
}
