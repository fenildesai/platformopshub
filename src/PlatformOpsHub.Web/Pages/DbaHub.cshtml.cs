using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class DbaHubModel : PageModel
    {
        private readonly IMediator _mediator;

        public DbaHubModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<DbaMaintenanceDto> MaintenanceTasks { get; private set; } = new();

        public async Task OnGetAsync()
        {
            MaintenanceTasks = await _mediator.Send(new GetDbaMaintenanceQuery());
        }
    }
}
