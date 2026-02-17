using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Epics.Queries;

public record GetEpicsQuery : IRequest<List<EpicDto>>;

public class GetEpicsQueryHandler : IRequestHandler<GetEpicsQuery, List<EpicDto>>
{
    private readonly ApplicationDbContext _context;

    public GetEpicsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EpicDto>> Handle(GetEpicsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Epics
            .Include(e => e.Team)
            .OrderByDescending(e => e.UpdatedAt)
            .Select(e => new EpicDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                TeamName = e.Team.Name,
                Status = e.Status,
                JiraKey = e.JiraKey,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
