using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
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
