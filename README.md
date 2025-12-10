<div align="center">
  <h1>Eventra</h1>
  <p><strong>Plataforma de Gerenciamento de Eventos e Inscrições</strong></p>
  
  <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET_9-512BD4?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/MySQL-4479A1?style=flat-square&logo=mysql&logoColor=white" />
  <img src="https://img.shields.io/badge/Next.js_14-000000?style=flat-square&logo=nextdotjs&logoColor=white" />
  <img src="https://img.shields.io/badge/TypeScript-3178C6?style=flat-square&logo=typescript&logoColor=white" />
  <img src="https://img.shields.io/badge/Tailwind_CSS-06B6D4?style=flat-square&logo=tailwindcss&logoColor=white" />
</div>

---

## Sobre o Projeto

**Eventra** é uma plataforma para criação, gerenciamento e inscrição em eventos. Sistema completo com backend REST API em .NET 9 e frontend em Next.js 14.

### Principais Features

- 🔐 Autenticação JWT com 3 tipos de usuários (Admin, Organizer, Participant)
- 📍 Localização de eventos via Google Place ID
- 🎟️ Sistema de inscrições com controle de vagas
- 🔒 Segurança com BCrypt e validações de autorização
- ⚡ Transações garantindo integridade dos dados

---

## Tecnologias

### Backend
- ASP.NET Core 9.0
- Entity Framework Core
- MySQL 8.0
- JWT Authentication
- BCrypt

### Frontend
- Next.js 14
- TypeScript
- Tailwind CSS
- Axios
- Context API

### Integrações
- Google Places API (localização via Place ID)

---

## Estrutura

```
Eventra/
├── backend/
│   ├── Controllers/        # Endpoints (Users, Events, Registrations)
│   ├── Services/           # Lógica de negócio
│   ├── Models/             # Entidades
│   ├── DTOs/               # Data Transfer Objects
│   └── Data/               # EF Core DbContext
└── frontend/
    └── src/
        ├── app/            # Next.js App Router
        ├── components/     # Componentes React
        ├── contexts/       # AuthContext
        ├── services/       # Chamadas API (Axios)
        └── types/          # Interfaces TypeScript
```

---

## Como Rodar

### Pré-requisitos
- .NET 9 SDK
- MySQL 8.0+
- Node.js 18+
- Chave da Google Places API (opcional, para autocomplete de endereço)

### Backend

```bash
git clone https://github.com/GabrielSilvaVG/Eventra.git
cd Eventra/backend
```

Configure `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Eventra_DB;Uid=root;Pwd=sua_senha;"
  }
}
```

Execute:
```bash
dotnet ef database update
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run
```

**API:** `https://localhost:5001` | **Swagger:** `https://localhost:5001/swagger`

### Frontend
```bash
cd frontend
npm install
npm run dev
```

**App:** `http://localhost:3000`

---

## API - Endpoints

### Usuários
| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| POST | `/api/users/register` | Cadastrar | Público |
| POST | `/api/users/login` | Login (retorna JWT) | Público |
| GET | `/api/users/{id}` | Buscar usuário | Próprio/Admin |
| GET | `/api/users` | Listar todos | Admin |
| PUT | `/api/users/{id}` | Atualizar | Próprio/Admin |
| DELETE | `/api/users/{id}` | Deletar | Próprio/Admin |

### Eventos
| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| GET | `/api/events` | Listar eventos | Público |
| GET | `/api/events/{id}` | Detalhes | Público |
| POST | `/api/events` | Criar | Organizer/Admin |
| PUT | `/api/events/{id}` | Editar | Dono/Admin |
| DELETE | `/api/events/{id}` | Deletar | Dono/Admin |

### Inscrições
| Método | Rota | Descrição | Acesso |
|--------|------|-----------|--------|
| POST | `/api/registrations` | Inscrever-se | Participant |
| GET | `/api/registrations/my-registrations` | Minhas inscrições | Participant |
| GET | `/api/registrations/event/{eventId}` | Inscritos no evento | Autenticado |
| DELETE | `/api/registrations/{id}` | Cancelar | Próprio/Admin |

---

## Modelos de Dados

### Event
```typescript
{
  id: number
  title: string
  type: EventType          // Conference, Workshop, Seminar, etc.
  placeId: string          // Google Place ID para localização
  status: EventStatus      // Scheduled, Ongoing, Completed, Cancelled
  date: DateTime
  description?: string
  totalSlots: number
  availableSlots: number
  organizer: { id, name, email }
}
```

### User
```typescript
{
  id: number
  name: string
  email: string
  userType: UserType       // Admin (0), Organizer (1), Participant (2)
}
```

---

## Permissões

| Ação | Admin | Organizer | Participant |
|------|-------|-----------|-------------|
| Gerenciar usuários | ✅ | ❌ | ❌ |
| Criar eventos | ✅ | ✅ | ❌ |
| Editar qualquer evento | ✅ | ❌ | ❌ |
| Editar próprio evento | ✅ | ✅ | ❌ |
| Inscrever-se | ❌ | ❌ | ✅ |

---

## Arquitetura

```
Frontend (Next.js) → API REST → Controllers → Services → EF Core → MySQL
                        ↓
                   JWT + Roles
```

**Padrões:** Service Layer • Dependency Injection • DTOs • Role-based Auth

---

## Roadmap

- [x] Backend MVP
- [x] Autenticação JWT
- [x] Sistema de inscrições
- [x] Integração Google Place ID
- [ ] Frontend completo
- [ ] Deploy

---

<div align="center">
  <sub>Desenvolvido por <a href="https://github.com/GabrielSilvaVG">Gabriel Silva</a></sub>
</div>

