# Task Management System (ASP.NET Core Razor Pages)

This solution is built from the schema in `TaskManagementDB.sql` and provides a full Razor Pages app with:

- Cookie login/logout
- Role-based page permission checks (`UserRole`, `AppPage`, `RolePagePermission`)
- Dashboard
- CRUD pages for all schema tables:
  - User Roles
  - Users
  - Client Profiles
  - App Pages
  - Role Permissions
  - Categories
  - Tasks
  - Task Updates
- Startup seeding for roles, sample users, pages, permissions, category, task, and update

## Project Structure

- `TaskManagementSystem.slnx`
- `TaskManagementSystem.Web/`

## Default Login Users

- Admin: `hasan` / `hasan123`
- Employee: `hasan.emp` / `hasan123`
- Client: `hasan.client` / `hasan123`
- Manager: `hasan.manager` / `hasan123`

## Database

Connection string is in:

- `TaskManagementSystem.Web/appsettings.json`

Default value uses LocalDB:

`Server=(localdb)\\mssqllocaldb;Database=TaskManagementDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`

If needed, change it to your SQL Server instance.

## Run

From repository root:

```bash
dotnet restore
dotnet build
dotnet run --project .\TaskManagementSystem.Web\TaskManagementSystem.Web.csproj --urls http://localhost:5050
```

Then open `http://localhost:5050`.

## Notes

- The app uses `EnsureCreated` + startup seed in `Data/DatabaseSeeder.cs`.
- If the default launch URL port is already used, pass another URL with `--urls`.
