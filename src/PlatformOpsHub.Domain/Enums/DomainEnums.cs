namespace PlatformOpsHub.Domain.Enums;

public enum TeamType
{
    Service = 1,
    Change = 2,
    DBA = 3
}

public enum EpicStatus
{
    Backlog = 1,
    InProgress = 2,
    Blocked = 3,
    Done = 4
}

public enum Environment
{
    Dev = 1,
    Sys = 2,
    Rwy = 3,
    Staging = 4,
    Preprod = 5,
    Prod = 6
}

public enum KpiCategory
{
    SecurityAndCompliance = 1,
    Sustainability = 2,
    Productivity = 3,
    PerformanceAndEfficiency = 4,
    CostObservability = 5,
    CostOptimisation = 6,
    AverageBuildTime = 7,
    TimeToRelease = 8,
    MTTR = 9,
    ChangeFailureRate = 10,
    EngineeringPractices = 11,
    Growth = 12,
    Utilities = 13,
    ApplicationAndInfrastructure = 14,
    ProcessQualityAndGovernance = 15,
    GenAIInitiatives = 16
}

public enum UnitType
{
    Number = 1,
    Percentage = 2,
    Boolean = 3,
    Duration = 4
}

public enum IntegrationType
{
    AzureDevOps = 1,
    Jira = 2,
    AzureCost = 3,
    SonarQube = 4,
    Checkmarx = 5,
    TeamsWebhook = 6,
    MicrosoftGraph = 7,
    AzureOpenAI = 8
}

public enum NotificationChannel
{
    Teams = 1,
    Email = 2
}

public enum DeploymentResult
{
    Success = 1,
    Fail = 2,
    Partial = 3
}

public enum PipelineStatus
{
    Queued = 1,
    Running = 2,
    Succeeded = 3,
    Failed = 4,
    Canceled = 5
}

public enum ScopeType
{
    Subscription = 1,
    ResourceGroup = 2,
    Product = 3
}

public enum CodeQualitySource
{
    SonarQube = 1,
    Checkmarx = 2
}

public enum DbEnvironment
{
    NonProd = 1,
    Prod = 2
}

public enum QuizQuestionType
{
    SingleChoice = 1,
    MultipleChoice = 2
}

public enum NewsletterPeriod
{
    Weekly = 1,
    Monthly = 2
}

public enum MilestoneType
{
    Birthday = 1,
    Anniversary = 2,
    TeamEvent = 3
}
