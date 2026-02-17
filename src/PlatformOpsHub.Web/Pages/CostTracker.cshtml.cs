using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class CostTrackerModel : PageModel
    {
        private readonly IMediator _mediator;

        public CostTrackerModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CostOptimizationDto> WorkItems { get; private set; } = new();

        public async Task OnGetAsync()
        {
            WorkItems = await _mediator.Send(new GetCostOptimizationWorkQuery());
        }
    }
}
