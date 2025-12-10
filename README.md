<div align="center">
  <h1>Eventra API</h1>
  <p><strong>REST API para Gerenciamento de Eventos e Inscrições</strong></p>
  
  <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET_9-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/MySQL-4479A1?style=flat-square&logo=mysql&logoColor=white" />
  <img src="https://img.shields.io/badge/JWT-000000?style=flat-square&logo=jsonwebtokens&logoColor=white" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black" />
</div>

---

## Sobre o Projeto

**Eventra** é uma API REST para criação, gerenciamento e inscrição em eventos. Desenvolvida em .NET 9 com arquitetura em camadas, autenticação JWT e documentação Swagger.

### Features

- 🔐 **Autenticação JWT** com Access Token + Refresh Token
- 👥 **3 tipos de usuários**: Admin, Organizer, Participant
- 📍 **Localização** via Google Place ID
- 🎟️ **Inscrições** com controle de vagas
- 🔒 **Segurança**: BCrypt, validações de autorização, role-based access
- 📖 **Documentação** interativa com Swagger

---

## Tecnologias

- **ASP.NET Core 9.0** — Framework web
- **Entity Framework Core** — ORM
- **MySQL 8.0** — Banco de dados
- **JWT Bearer** — Autenticação
- **BCrypt** — Hash de senhas
- **Swagger** — Documentação da API

---

## Arquitetura

```
Controllers  →  Services  →  DbContext  →  MySQL
     ↓              ↓
   DTOs         Models
```

```
backend/
├── Controllers/     # Endpoints da API
├── Services/        # Lógica de negócio
├── Models/          # Entidades do banco
├── DTOs/            # Data Transfer Objects
├── Data/            # DbContext + Migrations
├── Enums/           # EventType, EventStatus, UserType
└── Extensions/      # Extension methods
```

---

## Como Rodar

### Pré-requisitos
- .NET 9 SDK
- MySQL 8.0+

### Instalação

```bash
git clone https://github.com/GabrielSilvaVG/Eventra.git
cd Eventra/backend
```

Configure `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Eventra_DB;Uid=root;Pwd=sua_senha;"
  },
  "Jwt": {
    "Key": "sua-chave-secreta-com-pelo-menos-32-caracteres",
    "Issuer": "Eventra",
    "Audience": "Eventra",
    "ExpirationInMinutes": 60
  }
}
```

Execute:
```bash
dotnet ef database update
dotnet run
```

### Swagger
Acesse: `http://localhost:5000/swagger`

---

## API Endpoints

### 🔐 Autenticação

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/users/register` | Cadastrar usuário |
| POST | `/api/users/login` | Login (retorna tokens) |
| POST | `/api/users/refresh-token` | Renovar tokens |
| POST | `/api/users/logout` | Revogar refresh token |

### 👥 Usuários

| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| GET | `/api/users` | Listar todos | Admin |
| GET | `/api/users/{id}` | Buscar por ID | Próprio/Admin |
| PUT | `/api/users/{id}` | Atualizar | Próprio/Admin |
| DELETE | `/api/users/{id}` | Deletar | Próprio/Admin |

### 📅 Eventos

| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| GET | `/api/events` | Listar eventos | Público |
| GET | `/api/events/{id}` | Detalhes | Público |
| POST | `/api/events` | Criar | Organizer/Admin |
| PUT | `/api/events/{id}` | Editar | Dono/Admin |
| DELETE | `/api/events/{id}` | Deletar | Dono/Admin |

### 🎟️ Inscrições

| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| POST | `/api/registrations` | Inscrever-se | Participant |
| GET | `/api/registrations` | Listar todas | Admin |
| GET | `/api/registrations/{id}` | Buscar por ID | Autenticado |
| GET | `/api/registrations/my-registrations` | Minhas inscrições | Participant |
| GET | `/api/registrations/event/{eventId}` | Inscritos no evento | Autenticado |
| DELETE | `/api/registrations/{id}` | Cancelar | Próprio/Admin |

---

## Modelos

### User
```json
{
  "id": 1,
  "name": "João Silva",
  "email": "joao@email.com",
  "userType": 2  // 0=Admin, 1=Organizer, 2=Participant
}
```

### Event
```json
{
  "id": 1,
  "title": "Tech Conference 2025",
  "type": 0,           // Conference, Workshop, Seminar, Meetup, Webinar
  "placeId": "ChIJ...", // Google Place ID
  "status": 0,         // Scheduled, Ongoing, Completed, Cancelled
  "date": "2025-12-20T14:00:00",
  "description": "Descrição do evento",
  "totalSlots": 100,
  "occupiedSlots": 45,
  "availableSlots": 55,
  "organizer": { "id": 1, "name": "Org", "email": "org@email.com" }
}
```

### AuthResponse (Login)
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "a1b2c3d4e5f6...",
  "user": { "id": 1, "name": "João", "email": "...", "userType": 2 }
}
```

---

## Permissões

| Ação | Admin | Organizer | Participant |
|------|:-----:|:---------:|:-----------:|
| Gerenciar usuários | ✅ | ❌ | ❌ |
| Criar eventos | ✅ | ✅ | ❌ |
| Editar qualquer evento | ✅ | ❌ | ❌ |
| Editar próprio evento | ✅ | ✅ | ❌ |
| Inscrever-se em eventos | ❌ | ❌ | ✅ |
| Ver todas as inscrições | ✅ | ❌ | ❌ |

---

## Autenticação

### Fluxo JWT com Refresh Token

```
1. POST /login → { accessToken (60min), refreshToken (7 dias) }
2. Usar accessToken nas requisições: Authorization: Bearer <token>
3. Quando accessToken expirar (401):
   POST /refresh-token → { novo accessToken, novo refreshToken }
4. POST /logout → revoga refreshToken
```

### Exemplo de uso

```bash
# Login
curl -X POST https://localhost:5001/api/users/login \
  -H "Content-Type: application/json" \
  -d '{"email": "user@email.com", "password": "123456"}'

# Requisição autenticada
curl https://localhost:5001/api/events \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIs..."

# Refresh token
curl -X POST https://localhost:5001/api/users/refresh-token \
  -H "Content-Type: application/json" \
  -d '{"refreshToken": "a1b2c3d4e5f6..."}'
```

---

## Admin Padrão

Ao rodar as migrations, é criado um admin:

- **Email:** `admin@Eventra.com`
- **Senha:** `Admin@123`

---

> **Stack:** ASP.NET Core 9.0 | Entity Framework Core | MySQL 8.0 | JWT Authentication
