# NovaPulse — .NET 8 Blazor & Entity Framework Core Workspace

A modern, high-performance web application built with **.NET 8.0**, **Blazor Server (Interactive Server Render Mode)**, and **Entity Framework Core with SQLite**.

---

## 🚀 Key Features

- **Blazor Server 8.0 Interactive Mode**: Real-time bidirectional UI updates over WebSockets (SignalR) with instant DOM diffing.
- **Entity Framework Core 8 with SQLite**:
  - Thread-safe `IDbContextFactory<AppDbContext>` design pattern.
  - Automatic database initialization and fixture seeding on startup.
  - Comprehensive CRUD operations for Projects & Tasks with audit logging (`ActivityLogs`).
- **Executive Dashboard**:
  - Real-time KPI metric cards (Total Initiatives, In Progress, In Review, Backlog, Completed, Critical Alert).
  - Progress percentage aggregators and hours tracking (Estimated vs Logged).
  - Quick action launcher for creating projects and exploring schemas.
  - Live activity event stream.
- **Full-featured Project & Task Manager (`/projects`)**:
  - Filter by Status tabs (All, In Progress, In Review, Backlog, Completed).
  - Keyword search (by title, assignee, description, tags).
  - Category and Priority dropdown filters + Dynamic sorting.
  - Switch between **Card Grid** view and **Data Table** view.
  - In-place status updates, star/favorite toggle, creation & editing modals with validation, and deletion confirmation dialogs.
- **Engineering Analytics & Velocity (`/analytics`)**:
  - Overall completion rate, workload efficiency ratio, and category distribution.
  - Priority and status meter breakdown.
  - Team lead / assignee workload balance table.
- **Database & EF Core Explorer (`/database`)**:
  - Live SQLite connection health and record counts.
  - Persisted audit trail inspection.
  - One-click database reset and re-seed mechanism.
- **Reactive State (`/counter`) & Weather Forecaster (`/weather`)**:
  - Custom step counter with mutation history log.
  - Global meteorological forecast with city switcher and °C/°F toggle.
- **Modern Design System**:
  - Deep slate & midnight indigo glassmorphism theme (`app.css`).
  - Google Fonts typography (*Plus Jakarta Sans* & *JetBrains Mono*).
  - Floating toast notification system (`ToastContainer.razor`).

---

## 🛠️ Tech Stack & Architecture

- **Runtime**: .NET 8.0 SDK (`net8.0`)
- **Web Framework**: ASP.NET Core 8 Blazor (`Microsoft.NET.Sdk.Web`)
- **Data Access**: `Microsoft.EntityFrameworkCore.Sqlite` (8.0.11)
- **Icons**: Bootstrap Icons (1.11.3)
- **Database**: SQLite 3 (`novapulse.db`)

---

## 📂 Project Structure

```
.net/
├── Components/
│   ├── App.razor                     # Root HTML host page with font & CSS imports
│   ├── Routes.razor                  # Blazor router configuration
│   ├── _Imports.razor                # Global namespaces and usings
│   ├── Layout/
│   │   ├── MainLayout.razor          # Header topbar, breadcrumb, and layout wrapper
│   │   └── NavMenu.razor             # Sidebar navigation with active indicators
│   ├── Pages/
│   │   ├── Home.razor                # Executive KPI Dashboard
│   │   ├── Projects.razor            # Project & Task management (Grid/Table + CRUD)
│   │   ├── Analytics.razor           # Telemetry & engineering velocity metrics
│   │   ├── DatabaseInfo.razor        # EF Core schema explorer & DB reset
│   │   ├── Counter.razor             # Reactive state management demo
│   │   └── Weather.razor             # Meteorological insights & forecasts
│   └── Shared/
│       ├── ConfirmModal.razor        # Reusable action confirmation modal
│       ├── ProjectModal.razor        # Create/Edit project modal with validation
│       └── ToastContainer.razor      # Global floating toast notifications
├── Data/
│   ├── AppDbContext.cs               # EF Core DbContext with Fluent API mappings
│   ├── DbInitializer.cs              # Database auto-creation and seed fixtures
│   └── Models/
│       ├── ActivityLog.cs            # Audit log entity
│       ├── Enums.cs                  # Category, Priority, and Status enums
│       └── ProjectItem.cs            # Project entity with DataAnnotations
├── Services/
│   ├── IProjectService.cs            # Service contract & DashboardStats model
│   ├── ProjectService.cs             # Thread-safe EF Core implementation
│   └── ToastService.cs               # Global alert & notification event broker
├── Properties/
│   └── launchSettings.json           # Local development launch profiles
├── wwwroot/
│   ├── app.css                       # NovaPulse design system & glassmorphism
│   └── favicon.png
├── appsettings.json                  # Connection string & logging configuration
├── DotNetApp.csproj                  # Project file & package references
└── Program.cs                        # App entry point, DI, & pipeline configuration
```

---

## ⚡ How to Run

1. Open your terminal in the project directory:
   ```bash
   cd c:\xampp\htdocs\.net
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open your browser and navigate to the printed URL (e.g. `http://localhost:5281` or `https://localhost:7119`).
