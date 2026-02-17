using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Activities.Queries;

public record GetActivitiesQuery : IRequest<List<ActivityDto>>;

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, List<ActivityDto>>
{
    private readonly ApplicationDbContext _context;

    public GetActivitiesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ActivityDto>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Activities
            .Include(a => a.Team)
            .Include(a => a.Owner)
            .OrderByDescending(a => a.UpdatedAt)
            .Select(a => new ActivityDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                TeamName = a.Team.Name,
                Status = a.Status,
                OwnerName = a.Owner != null ? a.Owner.DisplayName : "Unassigned",
                DueAt = a.DueAt,
                Tags = a.Tags
            })
            .ToListAsync(cancellationToken);
    }
}
