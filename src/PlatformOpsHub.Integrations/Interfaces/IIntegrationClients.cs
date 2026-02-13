namespace PlatformOpsHub.Integrations.Interfaces;

public interface IAzureOpenAIClient
{
    Task<string> GenerateNewsletterAsync(string period, DateTime rangeStart, DateTime rangeEnd, string dataContext);
    Task<string> GetInsightAsync(string question, string context);
}

public interface ITeamsNotificationClient
{
    Task SendNotificationAsync(string webhookUrl, string adaptiveCardJson);
    Task SendBirthdayNotificationAsync(string displayName, string email);
    Task SendAnniversaryNotificationAsync(string displayName, int years);
    Task SendNewsletterNotificationAsync(string title, string url);
}

public interface IAzureDevOpsClient
{
    Task<List<PipelineBuildDto>> GetRecentBuildsAsync(int days = 7);
    Task<List<ReleaseDto>> GetRecentReleasesAsync(int days = 7);
    Task<List<PullRequestDto>> GetRecentPullRequestsAsync(int days = 7);
}

public interface IJiraClient
{
    Task<List<JiraIssueDto>> GetIssuesByProjectAsync(string projectKey);
    Task<JiraIssueDto?> GetIssueByKeyAsync(string issueKey);
}

public interface IAzureCostClient
{
    Task<List<CostDataDto>> GetCostDataAsync(DateTime startDate, DateTime endDate, string scope);
}

public interface ISonarQubeClient
{
    Task<SonarMetricsDto> GetProjectMetricsAsync(string projectKey);
    Task<List<SonarProjectDto>> GetAllProjectsAsync();
}

public interface ICheckmarxClient
{
    Task<CheckmarxScanDto> GetLatestScanAsync(string projectName);
    Task<List<CheckmarxProjectDto>> GetAllProjectsAsync();
}

// DTOs
public record PipelineBuildDto(string PipelineName, string BuildId, string Status, DateTime QueuedAt, DateTime? CompletedAt, int DurationSeconds);
public record ReleaseDto(string ReleaseName, string ReleaseId, string Environment, string Status, DateTime CreatedAt);
public record PullRequestDto(string Title, string Author, string Status, DateTime CreatedAt, int CommentsCount);
public record JiraIssueDto(string Key, string Summary, string Status, string Type, string? AssigneeEmail);
public record CostDataDto(DateTime Date, string ScopeId, decimal Amount, string Currency);
public record SonarMetricsDto(string ProjectKey, int Bugs, int Vulnerabilities, int CodeSmells, decimal? Coverage, int Criticals, int Highs, int Mediums, int Lows);
public record SonarProjectDto(string Key, string Name);
public record CheckmarxScanDto(string ProjectName, DateTime ScanDate, int TotalVulnerabilities, int Criticals, int Highs, int Mediums, int Lows);
public record CheckmarxProjectDto(string Name, string Id);
