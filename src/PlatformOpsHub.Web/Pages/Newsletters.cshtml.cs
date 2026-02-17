using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Newsletters.Queries;

namespace PlatformOpsHub.Web.Pages;

public class NewslettersModel : PageModel
{
    private readonly IMediator _mediator;

    public NewslettersModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public List<NewsletterDto> Newsletters { get; set; } = new();

    public async Task OnGetAsync()
    {
        Newsletters = await _mediator.Send(new GetNewslettersQuery());
    }
}
