using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class EpicDetailModel : PageModel
    {
        private readonly IMediator _mediator;

        public EpicDetailModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public EpicDetailDto? Epic { get; private set; }

        public async Task OnGetAsync(int id)
        {
            Epic = await _mediator.Send(new GetEpicDetailQuery(id));
        }
    }
}
