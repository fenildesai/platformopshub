using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Domain.Enums;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetDashboardStatsQuery : IRequest<DashboardStatsDto>;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStatsDto>
{
    private readonly ApplicationDbContext _context;

    public GetDashboardStatsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var epics = await _context.Epics
            .GroupBy(e => e.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var weekAgo = DateTime.UtcNow.AddDays(-7);
        var deploymentsThisWeek = await _context.Deployments
            .CountAsync(d => d.DeployedAt >= weekAgo, cancellationToken);

        var recentRegression = await _context.RegressionRuns
            .OrderByDescending(r => r.RunAt)
            .FirstOrDefaultAsync(cancellationToken);

        var costTarget = await _context.CostTargets
            .FirstOrDefaultAsync(ct => ct.Year == 2026, cancellationToken);
        
        var costActualsYTD = (await _context.CostActuals
            .Where(ca => ca.Date.Year == 2026)
            .Select(ca => ca.Amount)
            .ToListAsync(cancellationToken))
            .Sum();

        var latestQuality = await _context.CodeQualitySnapshots
            .OrderByDescending(s => s.SnapshotWeekStart)
            .Take(10)
            .ToListAsync(cancellationToken);

        return new DashboardStatsDto
        {
            BacklogEpics = epics.FirstOrDefault(e => e.Status == EpicStatus.Backlog)?.Count ?? 0,
            InProgressEpics = epics.FirstOrDefault(e => e.Status == EpicStatus.InProgress)?.Count ?? 0,
            DoneEpics = epics.FirstOrDefault(e => e.Status == EpicStatus.Done)?.Count ?? 0,
            
            DeploymentsThisWeek = deploymentsThisWeek,
            NightlyRegressionPassRate = recentRegression != null && recentRegression.Total > 0 
                ? (double)recentRegression.Passed / recentRegression.Total * 100 
                : 0,
            
            PipelineHealthPercentage = 95.5, // Placeholder for internal logic
            
            AnnualCostTarget = costTarget?.AnnualTarget ?? 350000m,
            CostYTD = costActualsYTD,
            CostVariance = (costTarget?.AnnualTarget ?? 350000m) - costActualsYTD,
            
            NewBugsCount = latestQuality.Sum(s => s.Bugs),
            NewVulnerabilitiesCount = latestQuality.Sum(s => s.Vulns),
            AverageCodeCoverage = latestQuality.Any() ? (double)latestQuality.Average(s => s.CoveragePct ?? 0m) : 0,
            
            MeanTimeToRecoverHours = 2.4,
            LeadTimeDays = 5.2,
            ChangeFailureRatePercentage = 8.5,
            
            GenAIInitiativesCount = await _context.Activities.CountAsync(a => a.Team.Type == TeamType.Change && a.Tags != null && a.Tags.Contains("GenAI"), cancellationToken),
            DbaMaintenanceBacklogCount = await _context.DbaMaintenances.CountAsync(m => m.CompletedAt == null, cancellationToken)
        };
    }
}
