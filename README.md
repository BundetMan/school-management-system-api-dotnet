# 🏫 School Registration Management System — Backend API

ASP.NET Core REST API for managing student registration, class assignment, teacher management, scheduling, and payment tracking.

---

## Tech Stack

- **Framework:** ASP.NET Core 10
- **ORM:** Entity Framework Core
- **Database:** MS SQL Server
- **Auth:** JWT Bearer tokens + Role-based access control
- **Excel Export:** ClosedXML / EPPlus
- **PDF Export:** iTextSharp
- **Background Jobs:** Hosted Services / Hangfire

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MS SQL Server

### Setup

```bash
git clone https://github.com/your-org/school-registration-api.git
cd school-registration-api

cp appsettings.Example.json appsettings.Development.json
# Edit appsettings.Development.json with your DB connection string

dotnet restore
dotnet ef database update
dotnet run
```

API will be available at `https://localhost:5001`  
Swagger UI at `https://localhost:5001/swagger`

---

## Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=school_db;User=root;Password=yourpassword;"
  },
  "Jwt": {
    "Secret": "your-secret-key",
    "ExpiresInMinutes": 60
  }
}
```

---

## API Modules

| Module | Base Route | Description |
|---|---|---|
| Auth | `/api/auth` | Login, session, role management |
| Students | `/api/students` | Registration, profiles, guardian info |
| Classes | `/api/classes` | Class CRUD, capacity, subject assignment |
| Teachers | `/api/teachers` | Teacher profiles, subject/class assignment |
| Payments | `/api/payments` | Record, verify, and track payments |
| Schedules | `/api/schedules` | Auto-generate and manage timetables |
| Reports | `/api/reports` | Enrollment, payment, waitlist exports |
| Waitlists | `/api/waitlists` | Queue management when class is full |

---

## Roles

| Role | Access |
|---|---|
| `admin` | Full access |
| `teacher` | Registration, payments, class assignment |

---

## Project Structure

```
SchoolAPI/
├── BuildHost-net472/        # .NET Framework 4.7.2 build host
├── BuildHost-netcore/       # .NET Core build host
├── Controllers/             # API route handlers
├── Data/                    # DbContext and data access setup
├── DTOs/                    # Request and response models
├── Mappings/                # AutoMapper profiles
├── Middlewares/             # Custom middleware (auth, error handling, etc.)
├── Migrations/              # EF Core database migrations
├── Models/                  # Domain entities
├── Repositories/            # Data access layer
├── Services/                # Business logic layer
├── schema.sql               # Raw SQL schema reference
├── SchoolAPI.http           # HTTP request samples for testing
├── appsettings.json         # App configuration
└── Program.cs               # App entry point and service registration
```

---

## Running Tests

```bash
dotnet test
```

---

## License

[MIT](./LICENSE)
