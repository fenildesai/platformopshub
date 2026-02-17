using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using PlatformOpsHub.Application.Features.Dashboard.Queries;
using PlatformOpsHub.Application.DTOs;

namespace PlatformOpsHub.Web.Pages
{
    public class ChangeTeamHubModel : PageModel
    {
        private readonly IMediator _mediator;

        public ChangeTeamHubModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public DashboardStatsDto Stats { get; private set; } = null!;

        public async Task OnGetAsync()
        {
            Stats = await _mediator.Send(new GetDashboardStatsQuery());
        }
    }
}
