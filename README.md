# ERMS - Event Registration Management System

Sistema de gerenciamento de eventos e inscrições

## Stack

**Backend:** ASP.NET Core Web API (.NET 9) + Entity Framework Core + MySQL  
**Frontend:** Next.js 14 + TypeScript + Tailwind CSS  
**Auth:** JWT Bearer tokens

## Estrutura

```
erms/
├── backend/        # API REST
└── frontend/       # Interface web
```

## Executar

```bash
# Backend
cd backend
dotnet ef database update
dotnet run

# Frontend
cd frontend
npm install
npm run dev
```

## Funcionalidades

- Autenticação (Admin/Organizer/Participant)
- CRUD de eventos
- Sistema de inscrições com controle de vagas
- Dashboard por tipo de usuário
