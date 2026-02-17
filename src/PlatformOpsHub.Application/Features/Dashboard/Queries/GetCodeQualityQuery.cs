using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetCodeQualityQuery : IRequest<List<CodeQualitySummaryDto>>;

public class GetCodeQualityQueryHandler : IRequestHandler<GetCodeQualityQuery, List<CodeQualitySummaryDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCodeQualityQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CodeQualitySummaryDto>> Handle(GetCodeQualityQuery request, CancellationToken cancellationToken)
    {
        var latestSnapshots = await _context.CodeQualitySnapshots
            .GroupBy(s => s.ProjectKey)
            .Select(g => g.OrderByDescending(s => s.SnapshotWeekStart).First())
            .ToListAsync(cancellationToken);

        return latestSnapshots.Select(s => new CodeQualitySummaryDto
        {
            ProjectKey = s.ProjectKey,
            Bugs = s.Bugs,
            Vulnerabilities = s.Vulns,
            CodeSmells = s.Smells,
            CoveragePct = (double)(s.CoveragePct ?? 0m),
            Status = s.Bugs > 0 || s.Vulns > 0 ? "Failing" : "Passing"
        }).ToList();
    }
}
