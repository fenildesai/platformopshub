using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using PlatformOpsHub.Infrastructure.Data;
using PlatformOpsHub.Infrastructure.Seed;
using PlatformOpsHub.Integrations.Configuration;
using PlatformOpsHub.Integrations.Interfaces;
using PlatformOpsHub.Integrations.Mock;
using PlatformOpsHub.Application;
using PlatformOpsHub.Background.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// Application Layer
builder.Services.AddApplication();

// Background Jobs
builder.Services.AddPlatformOpsBackground("App_Data/hangfire.db");

// Fluent UI Blazor
builder.Services.AddFluentUIComponents();

// Database - SQLite with path resolution for Azure App Service
var dbPath = builder.Environment.IsProduction()
    ? Path.Combine(Environment.GetEnvironmentVariable("HOME") ?? ".", "site", "wwwroot", "App_Data", "platformops.db")
    : Path.Combine(builder.Environment.ContentRootPath, "App_Data", "platformops.db");

Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Integration Settings
builder.Services.Configure<IntegrationSettings>(builder.Configuration.GetSection("Integrations"));
var integrationSettings = builder.Configuration.GetSection("Integrations").Get<IntegrationSettings>() ?? new IntegrationSettings();

// Register Integration Clients (Mock or Real based on UseMockData setting)
if (integrationSettings.UseMockData)
{
    builder.Services.AddSingleton<IAzureOpenAIClient, MockAzureOpenAIClient>();
    builder.Services.AddSingleton<ITeamsNotificationClient, MockTeamsNotificationClient>();
    builder.Services.AddSingleton<IAzureDevOpsClient, MockAzureDevOpsClient>();
    builder.Services.AddSingleton<IJiraClient, MockJiraClient>();
    builder.Services.AddSingleton<IAzureCostClient, MockAzureCostClient>();
    builder.Services.AddSingleton<ISonarQubeClient, MockSonarQubeClient>();
    builder.Services.AddSingleton<ICheckmarxClient, MockCheckmarxClient>();
}
else
{
    // TODO: Register real clients when credentials are provided
    // For now, fallback to mock
    builder.Services.AddSingleton<IAzureOpenAIClient, MockAzureOpenAIClient>();
    builder.Services.AddSingleton<ITeamsNotificationClient, MockTeamsNotificationClient>();
    builder.Services.AddSingleton<IAzureDevOpsClient, MockAzureDevOpsClient>();
    builder.Services.AddSingleton<IJiraClient, MockJiraClient>();
    builder.Services.AddSingleton<IAzureCostClient, MockAzureCostClient>();
    builder.Services.AddSingleton<ISonarQubeClient, MockSonarQubeClient>();
    builder.Services.AddSingleton<ICheckmarxClient, MockCheckmarxClient>();
}

// Database Seeder
builder.Services.AddScoped<DatabaseSeeder>();

// Application Insights (if configured)
if (!string.IsNullOrEmpty(builder.Configuration["APPINSIGHTS_CONNECTION_STRING"]))
{
    builder.Services.AddApplicationInsightsTelemetry();
}

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAbove", policy => policy.RequireRole("Admin", "Manager"));
    options.AddPolicy("MemberOrAbove", policy => policy.RequireRole("Admin", "Manager", "Member"));
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// Background Jobs
app.UsePlatformOpsBackground();

app.MapRazorComponents<PlatformOpsHub.Web.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
