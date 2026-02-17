using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Infrastructure.Data;

namespace PlatformOpsHub.Application.Features.Newsletters.Queries;

public record GetNewslettersQuery : IRequest<List<NewsletterDto>>;

public class GetNewslettersQueryHandler : IRequestHandler<GetNewslettersQuery, List<NewsletterDto>>
{
    private readonly ApplicationDbContext _context;

    public GetNewslettersQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NewsletterDto>> Handle(GetNewslettersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Newsletters
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NewsletterDto
            {
                Id = n.Id,
                Period = n.Period,
                RangeStart = n.RangeStart,
                RangeEnd = n.RangeEnd,
                Markdown = n.Markdown,
                Html = n.Html,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
