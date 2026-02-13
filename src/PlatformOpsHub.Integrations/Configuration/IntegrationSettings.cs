namespace PlatformOpsHub.Integrations.Configuration;

public class IntegrationSettings
{
    public bool UseMockData { get; set; } = true;

    public AzureDevOpsSettings AzureDevOps { get; set; } = new();
    public JiraSettings Jira { get; set; } = new();
    public AzureCostSettings AzureCost { get; set; } = new();
    public SonarQubeSettings SonarQube { get; set; } = new();
    public CheckmarxSettings Checkmarx { get; set; } = new();
    public TeamsSettings Teams { get; set; } = new();
    public GraphSettings Graph { get; set; } = new();
    public OpenAISettings OpenAI { get; set; } = new();
}

public class AzureDevOpsSettings
{
    public string? OrganizationUrl { get; set; }
    public string? PersonalAccessToken { get; set; }
}

public class JiraSettings
{
    public string? BaseUrl { get; set; }
    public string? Email { get; set; }
    public string? ApiToken { get; set; }
}

public class AzureCostSettings
{
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? Scope { get; set; }
}

public class SonarQubeSettings
{
    public string? BaseUrl { get; set; }
    public string? Token { get; set; }
}

public class CheckmarxSettings
{
    public string? BaseUrl { get; set; }
    public string? Token { get; set; }
}

public class TeamsSettings
{
    public string? WebhookGeneral { get; set; }
    public string? WebhookService { get; set; }
    public string? WebhookChange { get; set; }
    public string? WebhookDBA { get; set; }
}

public class GraphSettings
{
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public bool Enabled { get; set; }
}

public class OpenAISettings
{
    public string? Endpoint { get; set; }
    public string? Key { get; set; }
    public string? Deployment { get; set; }
    public string ApiVersion { get; set; } = "2024-02-15-preview";
}
