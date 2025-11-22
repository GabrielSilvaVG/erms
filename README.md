# ERMS - Event Registration Management System

Sistema de Gerenciamento de Eventos e Inscrições recriado em C# + Next.js

## Estrutura do Projeto

```
erms/
├── backend/                 # ASP.NET Core Web API
│   ├── Controllers/        # API Controllers
│   ├── Data/              # DbContext e Migrations
│   ├── DTOs/              # Data Transfer Objects
│   ├── Enums/             # Enumerações (Status, TipoUsuario)
│   ├── Migrations/        # Entity Framework Migrations
│   ├── Models/            # Entidades (Evento, Participante, Inscricao)
│   └── Services/          # Serviços (Auth, Business Logic)
│
└── frontend/               # Next.js + TypeScript + Tailwind
    ├── public/            # Arquivos estáticos
    └── src/
        ├── app/           # Pages (App Router)
        ├── components/    # Componentes React
        ├── contexts/      # Context API (Auth, etc)
        ├── hooks/         # Custom Hooks
        ├── services/      # API calls
        ├── types/         # TypeScript types
        └── utils/         # Funções utilitárias
```

## Tecnologias

### Backend
- C# / .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / LocalDB
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Next.js 14+ (App Router)
- TypeScript
- Tailwind CSS
- React Hooks
- Axios/Fetch API

## Sobre

✅ API REST moderna com documentação Swagger  
✅ Frontend separado com Next.js e Tailwind CSS  
✅ Entity Framework (ORM) - sem SQL direto  
✅ Autenticação JWT stateless  
✅ TypeScript para type safety  
✅ UI responsiva e moderna  
✅ Migrations versionadas do banco  
✅ Injeção de dependências nativa  

## Como executar

### Backend
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend
```bash
cd frontend
npm install
npm run dev
```

## Funcionalidades

- Cadastro e login de Participantes e Organizadores
- CRUD de Eventos
- Sistema de inscrições em eventos
- Controle de vagas disponíveis
- Dashboard para participantes e organizadores
- Histórico de eventos
