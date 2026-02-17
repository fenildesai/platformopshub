using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetDbaMaintenanceQuery : IRequest<List<DbaMaintenanceDto>>;

public class GetDbaMaintenanceQueryHandler : IRequestHandler<GetDbaMaintenanceQuery, List<DbaMaintenanceDto>>
{
    private readonly ApplicationDbContext _context;

    public GetDbaMaintenanceQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DbaMaintenanceDto>> Handle(GetDbaMaintenanceQuery request, CancellationToken cancellationToken)
    {
        return await _context.DbaMaintenances
            .OrderByDescending(m => m.PlannedAt)
            .Select(m => new DbaMaintenanceDto
            {
                Id = m.Id,
                Instance = m.Instance,
                Database = m.Database,
                TaskType = m.TaskType,
                PlannedAt = m.PlannedAt,
                Notes = m.Notes,
                CompletedAt = m.CompletedAt
            })
            .ToListAsync(cancellationToken);
    }
}
