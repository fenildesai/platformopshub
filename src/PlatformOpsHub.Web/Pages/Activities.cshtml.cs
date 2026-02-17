using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Activities.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class ActivitiesModel : PageModel
    {
        private readonly IMediator _mediator;

        public ActivitiesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ActivityDto> Activities { get; private set; } = new();

        public async Task OnGetAsync()
        {
            Activities = await _mediator.Send(new GetActivitiesQuery());
        }
    }
}
