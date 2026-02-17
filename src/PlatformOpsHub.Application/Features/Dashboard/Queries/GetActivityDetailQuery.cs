using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetActivityDetailQuery(int Id) : IRequest<ActivityDetailDto?>;

public class GetActivityDetailQueryHandler : IRequestHandler<GetActivityDetailQuery, ActivityDetailDto?>
{
    private readonly ApplicationDbContext _context;

    public GetActivityDetailQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActivityDetailDto?> Handle(GetActivityDetailQuery request, CancellationToken cancellationToken)
    {
        var activity = await _context.Activities
            .Include(a => a.Team)
            .Include(a => a.Owner)
            .Include(a => a.Epic)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (activity == null) return null;

        return new ActivityDetailDto
        {
            Id = activity.Id,
            Title = activity.Title,
            Description = activity.Description,
            TeamName = activity.Team.Name,
            Status = activity.Status,
            OwnerName = activity.Owner != null ? activity.Owner.DisplayName : "Unassigned",
            DueAt = activity.DueAt,
            Tags = activity.Tags,
            LongDescription = activity.Description // Mapping for now
        };
    }
}
