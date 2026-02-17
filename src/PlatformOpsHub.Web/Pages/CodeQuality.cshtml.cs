using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Application.Features.Dashboard.Queries;

namespace PlatformOpsHub.Web.Pages
{
    public class CodeQualityModel : PageModel
    {
        private readonly IMediator _mediator;

        public CodeQualityModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CodeQualitySummaryDto> QualitySummaries { get; private set; } = new();

        public async Task OnGetAsync()
        {
            QualitySummaries = await _mediator.Send(new GetCodeQualityQuery());
        }
    }
}
