using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class WhoWeAreModel : PageModel
    {
        private readonly IMediator _mediator;

        public WhoWeAreModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<TeamSummaryDto> Teams { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Teams = await _mediator.Send(new GetWhoWeAreQuery());
        }
    }
}
