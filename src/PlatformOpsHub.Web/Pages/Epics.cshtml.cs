using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Epics.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class EpicsModel : PageModel
    {
        private readonly IMediator _mediator;

        public EpicsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<EpicDto> Epics { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Epics = await _mediator.Send(new GetEpicsQuery());
        }
    }
}
