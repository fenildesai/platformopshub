using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Newsletters.Commands;
using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Web.Pages;

public class NewsletterGeneratorModel : PageModel
{
    private readonly IMediator _mediator;

    public NewsletterGeneratorModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public GenerateNewsletterRequestDto Request { get; set; } = new();

    public string? GeneratedMarkdown { get; set; }
    public bool IsGenerating { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostGenerateAsync()
    {
        IsGenerating = true;
        var result = await _mediator.Send(new GenerateNewsletterCommand(Request));
        GeneratedMarkdown = result.Markdown;
        IsGenerating = false;
        
        return Page();
    }
}
