using PlatformOpsHub.Domain.Enums;

namespace PlatformOpsHub.Application.DTOs;

public class NewsletterDto
{
    public int Id { get; set; }
    public NewsletterPeriod Period { get; set; }
    public DateTime RangeStart { get; set; }
    public DateTime RangeEnd { get; set; }
    public string Markdown { get; set; } = null!;
    public string Html { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class GenerateNewsletterRequestDto
{
    public NewsletterPeriod Period { get; set; }
    public string? CustomPrompt { get; set; }
}
