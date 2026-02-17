using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetEpicDetailQuery(int Id) : IRequest<EpicDetailDto?>;

public class GetEpicDetailQueryHandler : IRequestHandler<GetEpicDetailQuery, EpicDetailDto?>
{
    private readonly ApplicationDbContext _context;

    public GetEpicDetailQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EpicDetailDto?> Handle(GetEpicDetailQuery request, CancellationToken cancellationToken)
    {
        var epic = await _context.Epics
            .Include(e => e.Team)
            .Include(e => e.Activities)
                .ThenInclude(a => a.Owner)
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (epic == null) return null;

        return new EpicDetailDto
        {
            Id = epic.Id,
            Title = epic.Title,
            Description = epic.Description,
            TeamName = epic.Team.Name,
            Status = epic.Status,
            JiraKey = epic.JiraKey,
            UpdatedAt = epic.UpdatedAt,
            CreatedBy = epic.CreatedBy,
            Activities = epic.Activities.Select(a => new ActivityDto
            {
                Id = a.Id,
                Title = a.Title,
                Status = a.Status,
                OwnerName = a.Owner != null ? a.Owner.DisplayName : "Unassigned",
                DueAt = a.DueAt
            }).ToList()
        };
    }
}
