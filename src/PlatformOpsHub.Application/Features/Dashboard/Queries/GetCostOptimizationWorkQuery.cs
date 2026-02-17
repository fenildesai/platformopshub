using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;
using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetCostOptimizationWorkQuery : IRequest<List<CostOptimizationDto>>;

public class GetCostOptimizationWorkQueryHandler : IRequestHandler<GetCostOptimizationWorkQuery, List<CostOptimizationDto>>
{
    private readonly ApplicationDbContext _context;

    public GetCostOptimizationWorkQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CostOptimizationDto>> Handle(GetCostOptimizationWorkQuery request, CancellationToken cancellationToken)
    {
        // Fetch activities related to "Cost" or in the "Change Team" which handles cost
        return await _context.Activities
            .Include(a => a.Owner)
            .Include(a => a.Team)
            .Where(a => a.Team.Type == TeamType.Change || a.Title.Contains("Cost") || a.Description.Contains("Cost"))
            .OrderByDescending(a => a.UpdatedAt)
            .Select(a => new CostOptimizationDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description ?? "No description provided.",
                Status = a.Status.ToString(),
                OwnerName = a.Owner != null ? a.Owner.DisplayName : "Unassigned",
                EstimatedSavings = 0 // Mocking for now, could be pulled from KPI impacts
            })
            .ToListAsync(cancellationToken);
    }
}
