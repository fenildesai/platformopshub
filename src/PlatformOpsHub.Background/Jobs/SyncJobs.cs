using Hangfire;
using Microsoft.Extensions.Logging;
using PlatformOpsHub.Integrations.Interfaces;
using PlatformOpsHub.Infrastructure.Data;
using PlatformOpsHub.Domain.Entities;
using PlatformOpsHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace PlatformOpsHub.Background.Jobs;

public class SyncJobs
{
    private readonly ILogger<SyncJobs> _logger;
    private readonly IAzureDevOpsClient _adoClient;
    private readonly IJiraClient _jiraClient;
    private readonly IAzureCostClient _costClient;
    private readonly ISonarQubeClient _sonarClient;
    private readonly ICheckmarxClient _cxClient;
    private readonly ApplicationDbContext _context;

    public SyncJobs(
        ILogger<SyncJobs> logger,
        IAzureDevOpsClient adoClient,
        IJiraClient jiraClient,
        IAzureCostClient costClient,
        ISonarQubeClient sonarClient,
        ICheckmarxClient cxClient,
        ApplicationDbContext context)
    {
        _logger = logger;
        _adoClient = adoClient;
        _jiraClient = jiraClient;
        _costClient = costClient;
        _sonarClient = sonarClient;
        _cxClient = cxClient;
        _context = context;
    }

    [JobDisplayName("Nightly Sync: ADO, Jira, Cost, Quality")]
    public async Task RunNightlySync()
    {
        _logger.LogInformation("Starting nightly sync job...");

        // 1. Sync ADO Pipeline Runs
        var builds = await _adoClient.GetRecentBuildsAsync(7);
        foreach (var run in builds)
        {
            if (!await _context.PipelineRuns.AnyAsync(r => r.RunId == run.BuildId))
            {
                _context.PipelineRuns.Add(new PipelineRun
                {
                    PipelineName = run.PipelineName,
                    RunId = run.BuildId,
                    Status = run.Status switch { "Succeeded" => PipelineStatus.Succeeded, "Failed" => PipelineStatus.Failed, _ => PipelineStatus.Running },
                    DurationSeconds = run.DurationSeconds,
                    QueuedAt = run.QueuedAt,
                    CompletedAt = run.CompletedAt,
                    Env = Domain.Enums.Environment.Prod // Mock assumption
                });
            }
        }

        // 2. Sync Azure Costs
        var costs = await _costClient.GetCostDataAsync(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow, "DefaultScope");
        foreach (var cost in costs)
        {
            _context.CostActuals.Add(new CostActual
            {
                Date = cost.Date,
                Amount = cost.Amount,
                ScopeId = cost.ScopeId
            });
        }

        // 3. Sync SonarQube Snapshots
        var sonarProjects = new[] { "platform-web", "platform-api", "platform-ops" };
        foreach (var project in sonarProjects)
        {
            var metrics = await _sonarClient.GetProjectMetricsAsync(project);
            _context.CodeQualitySnapshots.Add(new CodeQualitySnapshot
            {
                Source = CodeQualitySource.SonarQube,
                ProjectKey = project,
                SnapshotWeekStart = DateTime.UtcNow.Date,
                Bugs = metrics.Bugs,
                Vulns = metrics.Vulnerabilities,
                Smells = metrics.CodeSmells,
                CoveragePct = metrics.Coverage
            });
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Nightly sync job completed successfully.");
    }
}
