using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Dashboard.Queries;

public record GetAppreciationLeaderboardQuery : IRequest<List<LeaderboardEntryDto>>;

public class GetAppreciationLeaderboardQueryHandler : IRequestHandler<GetAppreciationLeaderboardQuery, List<LeaderboardEntryDto>>
{
    private readonly ApplicationDbContext _context;

    public GetAppreciationLeaderboardQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaderboardEntryDto>> Handle(GetAppreciationLeaderboardQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(u => u.Team)
            .Include(u => u.KudosReceived)
            .OrderByDescending(u => u.TotalPoints)
            .Take(20)
            .Select(u => new LeaderboardEntryDto
            {
                UserId = u.Id,
                DisplayName = u.DisplayName,
                PhotoUrl = u.PhotoUrl,
                Points = u.TotalPoints,
                KudosReceivedCount = u.KudosReceived.Count,
                TeamName = u.Team != null ? u.Team.Name : "Platform",
                DailyStreak = (u.Id % 10) + 2, // Simulation
                RecentAchievements = new List<string> { "Jira Guru", "Scrum Master", "Team Spirit" } // Simulation
            })
            .ToListAsync(cancellationToken);
    }
}
