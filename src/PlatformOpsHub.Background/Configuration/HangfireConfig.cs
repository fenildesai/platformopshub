using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformOpsHub.Background.Jobs;

namespace PlatformOpsHub.Background.Configuration;

public static class HangfireConfig
{
    public static IServiceCollection AddPlatformOpsBackground(this IServiceCollection services, string dbPath)
    {
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSQLiteStorage(dbPath));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        // Job registration
        services.AddScoped<SyncJobs>();
        services.AddScoped<NewsletterJob>();

        return services;
    }

    public static void UsePlatformOpsBackground(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireCustomAuthorizationFilter() }
        });

        // Recurring jobs
        RecurringJob.AddOrUpdate<SyncJobs>("nightly-sync", x => x.RunNightlySync(), Cron.Daily(2));
        RecurringJob.AddOrUpdate<NewsletterJob>("weekly-newsletter", x => x.GenerateWeeklyNewsletter(), Cron.Weekly(DayOfWeek.Monday, 9));
    }
}

public class HangfireCustomAuthorizationFilter : Hangfire.Dashboard.IDashboardAuthorizationFilter
{
    public bool Authorize(Hangfire.Dashboard.DashboardContext context)
    {
        // In production, restrict to admins. For demo, true.
        return true; 
    }
}
