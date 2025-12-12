

---


## About the Project

**Eventra** is a REST API for creating, managing, and registering for events. Built with .NET 9, layered architecture, JWT authentication, and Swagger documentation.

### Features

- **JWT Authentication** with Access & Refresh Token
- **3 user types**: Admin, Organizer, Participant
- **Location** via Google Place ID
- **Registrations** with slot control
- **Security**: BCrypt, authorization validations, role-based access
- **Interactive documentation** with Swagger

---


## Technologies

- **ASP.NET Core 9.0** — Web framework
- **Entity Framework Core** — ORM
- **MySQL 8.0** — Database
- **JWT Bearer** — Authentication
- **BCrypt** — Password hashing
- **Swagger** — API documentation

---


## Architecture

```
Controllers  →  Services  →  DbContext  →  MySQL
     ↓              ↓
   DTOs         Models
```

```
backend/
├── Controllers/     # API endpoints
├── Services/        # Business logic
├── Models/          # Database entities
├── DTOs/            # Data Transfer Objects
├── Data/            # DbContext + Migrations
├── Enums/           # EventType, EventStatus, UserType
└── Extensions/      # Extension methods
```

---


## How to Run

### Prerequisites
- .NET 9 SDK
- MySQL 8.0+

### Installation

```bash
git clone https://github.com/GabrielSilvaVG/Eventra.git
cd Eventra/backend
```

Configure your `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Eventra_DB;Uid=root;Pwd=your_password;"
  },
  "Jwt": {
    "Key": "your-secret-key-with-at-least-32-characters",
    "Issuer": "Eventra",
    "Audience": "Eventra",
    "ExpirationInMinutes": 60
  }
}
```

Run:
```bash
dotnet ef database update
dotnet run
```

### Swagger
Access: `http://localhost:5000/swagger`

---


## API Endpoints

### Authentication

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/users/register` | Register user |
| POST | `/api/users/login` | Login (returns tokens) |
| POST | `/api/users/refresh-token` | Refresh tokens |
| POST | `/api/users/logout` | Revoke refresh token |

### Users

| Method | Route | Description | Access |
|--------|-------|-------------|--------|
| GET | `/api/users` | List all | Admin |
| GET | `/api/users/{id}` | Get by ID | Self/Admin |
| PUT | `/api/users/{id}` | Update | Self/Admin |
| DELETE | `/api/users/{id}` | Delete | Self/Admin |

### Events

| Method | Route | Description | Access |
|--------|-------|-------------|--------|
| GET | `/api/events` | List events | Public |
| GET | `/api/events/{id}` | Details | Public |
| POST | `/api/events` | Create | Organizer/Admin |
| PUT | `/api/events/{id}` | Edit | Owner/Admin |
| DELETE | `/api/events/{id}` | Delete | Owner/Admin |

### Registrations

| Method | Route | Description | Access |
|--------|-------|-------------|--------|
| POST | `/api/registrations` | Register | Participant |
| GET | `/api/registrations` | List all | Admin |
| GET | `/api/registrations/{id}` | Get by ID | Authenticated |
| GET | `/api/registrations/my-registrations` | My registrations | Participant |
| GET | `/api/registrations/event/{eventId}` | Event registrations | Authenticated |
| DELETE | `/api/registrations/{id}` | Cancel | Self/Admin |

---


## Models

### User
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john@email.com",
  "userType": 2  // 0=Admin, 1=Organizer, 2=Participant
}
```

### Event
```json
{
  "id": 1,
  "title": "Tech Conference 2025",
  "type": 0,
  "placeId": "ChIJ...",
  "status": 0,
  "date": "2025-12-20T14:00:00",
  "description": "Event description",
  "totalSlots": 100,
  "occupiedSlots": 45
}
```
