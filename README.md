# Ticketing System - Backend API

## 🚀 Features

- **JWT Authentication** - Secure token-based authentication
- **Role-Based Authorization** - Separate permissions for Users and Admins
- **Ticket Management** - Create, read, update ticket operations
- **Status Workflow** - Requested → Open/Reject → Closed

## 📋 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL database (or Supabase account)
- Code editor (Visual Studio, VS Code, or Rider)

## ⚙️ Installation & Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd TicketSys  # or your backend folder name
```

### 2. Configure Database Connection

Update `appsettings.json` with your database credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=YOUR_HOST;Port=6543;Database=postgres;Username=YOUR_USERNAME;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
  },
  "Jwt": {
    "Key": "your-super-secret-key-minimum-32-characters-long!",
    "Issuer": "TicketSys",
    "Audience": "TicketSys",
    "ExpireMinutes": 60
  }
}
```

**Important:** 
- Replace `YOUR_HOST`, `YOUR_USERNAME`, and `YOUR_PASSWORD` with your actual credentials
- Never commit `appsettings.json` with real credentials to version control

### 3. Install Dependencies

```bash
dotnet restore
```

### 4. Apply Database Migrations

```bash
# Create migration (if not exists)
dotnet ef migrations add InitialCreate

# Apply to database
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

## 📚 API Documentation

Once running, access Swagger UI at: `http://localhost:5004/swagger`

### Authentication Endpoints

| Endpoint | Description | Auth Required |
|----------|-------------|---------------|
| `/api/Auth/register` | Register new user | No |
| `/api/Auth/login` | Login and get JWT token | No |
| `/api/Auth/getCurrentUser` | Get current user info | Yes |

### Ticket Endpoints

| Endpoint | Description | Auth Required | Role |
|----------|-------------|---------------|------|
| `/api/Tickets/CreateTicket` | Create new ticket | Yes | User/Admin |
| `/api/Tickets/GetTickets` | Get tickets (own/all) | Yes | User/Admin |
| `/api/Tickets/GetTicketByTicketId/{id}` | Get ticket by ID | Yes | User/Admin |
| `/api/Tickets/UpdateTicketDetail/{id}` | Update ticket details | Yes | User (own tickets only) |
| `/api/Tickets/UpdateTicketStatus/{id}` | Update ticket status | Yes | Admin only |

## 🔐 Authentication

### How to Use JWT in SWAGGER

1. Login via `/api/Auth/login` endpoint
2. Copy the `token` from the response
3. Click the **"Authorize"** button at the top of Swagger UI
4. Enter: `Bearer YOUR_TOKEN_HERE`
5. Click "Authorize"
6. Now you can test protected endpoints

## 👥 User Roles

### User (Customer)
- Create tickets
- View own tickets
- Update own tickets (only if status = "Requested")

### Admin
- View all tickets from all users
- Update ticket status:
  - Requested → Open (approve)
  - Requested → Reject
  - Open → Closed

## 🔄 Ticket Status Workflow

```
Customer creates ticket
        ↓
    [Requested] ← Customer can edit here
        ↓
    Admin reviews
        ↓
   [Open] or [Reject]
        ↓
  (if Open) Admin resolves
        ↓
    [Closed]
```

## 🧪 Testing

### Test User Accounts

| Email | Password | Role |
|-------|----------|------|
| admin@test.com | Admin123! | Admin |
| customer@test.com | Customer123! | User |

### Example API Calls

**Register:**
```json
POST /api/Auth/register
{
  "email": "newuser@test.com",
  "password": "Password123!",
  "role": "User"
}
```

**Login:**
```json
POST /api/Auth/login
{
  "email": "user@example.com",
  "password": "User123!"
}
```

**Create Ticket:**
```json
POST /api/Tickets/CreateTicket
Headers: { "Authorization": "Bearer YOUR_TOKEN" }
{
  "title": "Login Issue",
  "description": "Cannot login to the system",
  "priority": "High"
}
```

**Update Status (Admin only):**
```json
PATCH /api/Tickets/UpdateTicketStatus/1
Headers: { "Authorization": "Bearer ADMIN_TOKEN" }
{
  "status": "Open"
}
```

## 📦 Dependencies

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```
