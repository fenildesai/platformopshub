using MediatR;
using Microsoft.EntityFrameworkCore;
using PlatformOpsHub.Application.DTOs;
using PlatformOpsHub.Domain.Entities;
using PlatformOpsHub.Domain.Enums;
using PlatformOpsHub.Infrastructure.Data;
using PlatformOpsHub.Integrations.Interfaces;

namespace PlatformOpsHub.Application.Features.Newsletters.Commands;

public record GenerateNewsletterCommand(GenerateNewsletterRequestDto Request) : IRequest<NewsletterDto>;

public class GenerateNewsletterCommandHandler : IRequestHandler<GenerateNewsletterCommand, NewsletterDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IAzureOpenAIClient _aiClient;

    public GenerateNewsletterCommandHandler(ApplicationDbContext context, IAzureOpenAIClient aiClient)
    {
        _context = context;
        _aiClient = aiClient;
    }

    public async Task<NewsletterDto> Handle(GenerateNewsletterCommand command, CancellationToken cancellationToken)
    {
        var period = command.Request.Period;
        var now = DateTime.UtcNow;
        var start = period == NewsletterPeriod.Weekly ? now.AddDays(-7) : now.AddDays(-30);

        // Aggregate Data for context
        var deployments = await _context.Deployments
            .Where(d => d.DeployedAt >= start)
            .OrderByDescending(d => d.DeployedAt)
            .ToListAsync(cancellationToken);

        var activities = await _context.Activities
            .Include(a => a.Owner)
            .Where(a => a.CompletedAt >= start || a.CreatedAt >= start || a.UpdatedAt >= start)
            .OrderByDescending(a => a.UpdatedAt)
            .ToListAsync(cancellationToken);

        var sprintSummaries = activities
            .Where(a => a.Status == EpicStatus.Done)
            .Select(a => $"- {a.Title} (Completed by {a.Owner?.DisplayName ?? "System"})")
            .ToList();

        var contextData = $@"
            Period: {period}
            Date Range: {start:yyyy-MM-dd} to {now:yyyy-MM-dd}
            Deployments: {deployments.Count}
            Completed Activities: {sprintSummaries.Count}
            Highlights: {string.Join("\n", sprintSummaries.Take(5))}
            Custom Context: {command.Request.CustomPrompt ?? "None provided"}
        ";

        var markdown = await _aiClient.GenerateNewsletterAsync(period.ToString(), start, now, contextData);

        var newsletter = new Newsletter
        {
            Period = period,
            RangeStart = start,
            RangeEnd = now,
            Markdown = markdown,
            Html = markdown.Replace("\n", "<br/>"), // Simple placeholder for rendering
            CreatedAt = now
        };

        _context.Newsletters.Add(newsletter);
        await _context.SaveChangesAsync(cancellationToken);

        return new NewsletterDto
        {
            Id = newsletter.Id,
            Period = newsletter.Period,
            RangeStart = newsletter.RangeStart,
            RangeEnd = newsletter.RangeEnd,
            Markdown = newsletter.Markdown,
            Html = newsletter.Html,
            CreatedAt = newsletter.CreatedAt
        };
    }
}
