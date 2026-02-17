# Leaderboard Automation Strategy

To ensure fairness and reduce manual overhead, the leaderboard points are calculated using automated signals from our existing engineering tools.

## 1. Daily Jira Updates (10 pts)
**Objective**: Encourage consistent ticket management and status visibility.

### How it works:
- **Jira Webhooks**: We configure Jira to send a webhook on every issue update. 
- **Activity Detection**: Our background service (`SyncJobs.cs`) filters these webhooks for specific changes:
    - Status transitions (e.g., In Progress -> Done).
    - New comments added to assigned tickets.
    - Work logs recorded.
- **Rules**:
    - Only the *first* update of the day for each user counts towards the 10 points.
    - This creates a "Daily Management" incentive.

## 2. Running Scrum (10 pts)
**Objective**: Reward disciplined adherence to team ceremonies.

### How it works:
- **Jira Agile API**: We query the `/rest/agile/1.0/sprint` endpoint.
- **Activity Detection**: 
    - **Sprint Starts/Ends**: Automated points when a Scrum Master starts or finishes a sprint.
    - **Daily Standup Signal**: Since standups aren't always in Jira, we look for **"Bulk Transitions"** occurring within the first hour of the working day (indicating a standup just happened).
    - **Calendar Integration**: If integrated with Outlook/Google Calendar, we look for "Standup" meetings with >80% attendance.
- **Rules**:
    - 10 points awarded to the whole team if the daily signals are met.

## 3. Implementation Path
1. **Infrastructure**: Add `IntegrationConfigs` for Jira API Keys.
2. **Background Jobs**: Implement a `GamificationSyncJob` that runs nightly to process the day's audit logs.
3. **Database**: Store a `UserActivityLog` to prevent double-counting and track streaks.
