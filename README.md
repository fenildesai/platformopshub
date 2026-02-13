# PlatformOps Hub

PlatformOps Hub is a centralized platform management dashboard built with .NET 8 Blazor Server and Fluent UI. It provides visibility into deployments, costs, code quality, and team culture across the Platform Engineering organization.

## Quick Start Guide

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)

### 2. Environment Configuration
1. Copy the `.env.example` file to a new file named `.env` in the root directory.
   ```powershell
   cp .env.example .env
   ```
2. By default, `Integrations__UseMockData` is set to `true`. This allows you to run the application immediately with demo data without needing any external API keys.

### 3. Database Initialization
Run the following command from the root directory to create the SQLite database and apply migrations:
```powershell
dotnet ef database update --project src/PlatformOpsHub.Infrastructure --startup-project src/PlatformOpsHub.Web
```

### 4. Running the Application
Start the Blazor web application:
```powershell
dotnet run --project src/PlatformOpsHub.Web
```
The application will be accessible at: `https://localhost:7197` (or the port specified in your console output).

### 5. Accessing the Dashboard
- **Admin Login**: `admin@platformops.local` / `Admin@123456`
- **Hangfire Dashboard**: `https://localhost:7197/hangfire` (for background job monitoring)

## Switching to Real Integrations
To use real data from your organization:
1. Update the values in your `.env` file (ADO PAT, Jira Token, Azure Client Secret, etc.).
2. Set `Integrations__UseMockData=false` in your `.env` file.
3. Restart the application.

## Project Structure
- `src/PlatformOpsHub.Web`: Blazor Server UI and API endpoints.
- `src/PlatformOpsHub.Application`: Core business logic, MediatR handlers, and DTOs.
- `src/PlatformOpsHub.Domain`: Entities, Enums, and core domain logic.
- `src/PlatformOpsHub.Infrastructure`: EF Core DbContext, Migrations, and Identity.
- `src/PlatformOpsHub.Integrations`: API clients (Mock and Real) for external tools.
- `src/PlatformOpsHub.Background`: Hangfire job registrations and processing.

---
*Developed as an enterprise-ready Platform Engineering solution.*
