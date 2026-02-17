using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class ActivityDetailModel : PageModel
    {
        private readonly IMediator _mediator;

        public ActivityDetailModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ActivityDetailDto? Activity { get; private set; }

        public async Task OnGetAsync(int id)
        {
            Activity = await _mediator.Send(new GetActivityDetailQuery(id));
        }
    }
}
