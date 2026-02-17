using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class LeaderboardModel : PageModel
    {
        private readonly IMediator _mediator;

        public LeaderboardModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<LeaderboardEntryDto> Leaderboard { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Leaderboard = await _mediator.Send(new GetAppreciationLeaderboardQuery());
        }
    }
}
