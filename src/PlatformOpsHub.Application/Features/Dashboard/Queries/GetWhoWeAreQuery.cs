using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetWhoWeAreQuery : IRequest<List<TeamSummaryDto>>;

public class GetWhoWeAreQueryHandler : IRequestHandler<GetWhoWeAreQuery, List<TeamSummaryDto>>
{
    private readonly ApplicationDbContext _context;

    public GetWhoWeAreQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeamSummaryDto>> Handle(GetWhoWeAreQuery request, CancellationToken cancellationToken)
    {
        return await _context.Teams
            .Include(t => t.Users)
            .OrderBy(t => t.Name)
            .Select(t => new TeamSummaryDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                Members = t.Users.Select(u => new UserSummaryDto
                {
                    Id = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Email,
                    Role = u.Role,
                    Squad = u.Squad,
                    Skills = u.Skills,
                    PhotoUrl = u.PhotoUrl
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}
