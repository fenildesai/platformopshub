using Microsoft.Extensions.Logging;
using PlatformOpsHub.Integrations.Interfaces;
using PlatformOpsHub.Infrastructure.Data;
using PlatformOpsHub.Domain.Entities;
using PlatformOpsHub.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace PlatformOpsHub.Background.Jobs;

public class NewsletterJob
{
    private readonly ILogger<NewsletterJob> _logger;
    private readonly IAzureOpenAIClient _aiClient;
    private readonly ITeamsNotificationClient _teamsClient;
    private readonly ApplicationDbContext _context;

    public NewsletterJob(
        ILogger<NewsletterJob> logger,
        IAzureOpenAIClient aiClient,
        ITeamsNotificationClient teamsClient,
        ApplicationDbContext context)
    {
        _logger = logger;
        _aiClient = aiClient;
        _teamsClient = teamsClient;
        _context = context;
    }

    public async Task GenerateWeeklyNewsletter()
    {
        _logger.LogInformation("Generating weekly newsletter using AI...");

        var start = DateTime.UtcNow.AddDays(-7);
        var end = DateTime.UtcNow;

        // Fetch weekly highlights
        var deployments = await _context.Deployments.CountAsync(d => d.DeployedAt >= start);
        var bugsResolved = await _context.Activities.CountAsync(a => a.CompletedAt >= start && a.Title.Contains("Bug"));
        
        var dataContext = $"Stats: {deployments} deployments, {bugsResolved} bugs resolved.";
        
        var newsletterContent = await _aiClient.GenerateNewsletterAsync("Weekly", start, end, dataContext);

        var newsletter = new Newsletter
        {
            Period = NewsletterPeriod.Weekly,
            RangeStart = start,
            RangeEnd = end,
            Markdown = newsletterContent,
            Html = $"<div>{newsletterContent.Replace("\n", "<br/>")}</div>", // Simple conversion
            CreatedAt = DateTime.UtcNow
        };

        _context.Newsletters.Add(newsletter);
        await _context.SaveChangesAsync();

        // Notify Teams
        await _teamsClient.SendNewsletterNotificationAsync("Weekly PlatformOps Newsletter", "https://platformops-hub.azurewebsites.net/newsletters");
        
        _logger.LogInformation("Weekly newsletter generated and notification sent.");
    }
}
