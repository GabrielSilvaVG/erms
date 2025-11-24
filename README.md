<div align="center">
  <h1> Eventra</h1>
  <p><strong>API REST para Gerenciamento de Eventos e Inscrições</strong></p>
  
  <img src="https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET-6F42C1?style=flat-square&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/MySQL-4B7DA9?style=flat-square&logo=mysql&logoColor=white" />
  <img src="https://img.shields.io/badge/Next.js-222222?style=flat-square&logo=nextdotjs&logoColor=white" />
  <img src="https://img.shields.io/badge/TypeScript-3178C6?style=flat-square&logo=typescript&logoColor=white" />
  <img src="https://img.shields.io/badge/Tailwind%20CSS-38B2AC?style=flat-square&logo=tailwindcss&logoColor=white" />
</div>

---

##  Sobre o Projeto

**Eventra** é uma plataforma para criação, gerenciamento e inscrição em eventos. Sistema completo com backend REST API em .NET 9 e frontend em Next.js 14 (em desenvolvimento).

###  Principais Features

-  Autenticação JWT com 3 tipos de usuários (Admin, Organizer, Participant)
-  Sistema de inscrições com controle de vagas
-  Segurança com BCrypt e validações de autorização
-  Transações garantindo integridade dos dados
-  Arquitetura em camadas seguindo boas práticas

---

##  Tecnologias

**Backend:** ASP.NET Core 9.0 • Entity Framework Core • MySQL 8.0 • JWT • BCrypt

**Frontend:** Next.js 14 • TypeScript • Tailwind CSS *(em desenvolvimento)*

---

##  Estrutura

```
Eventra/
├── backend/
│   ├── Controllers/        # Endpoints (Users, Events, Registrations)
│   ├── Services/          # Lógica de negócio
│   ├── Models/            # Entidades
│   ├── DTOs/              # Data Transfer Objects
│   └── Data/              # EF Core DbContext
└── frontend/
    └── src/               # Next.js App Router
```

---

##  Como Rodar

### Pré-requisitos
- .NET 9 SDK
- MySQL 8.0+
- Node.js 18+ (para o frontend)

### Backend

1. Clone e configure:
```bash
git clone https://github.com/GabrielSilvaVG/Eventra.git
cd Eventra/backend
```

2. Edite `appsettings.json` com suas credenciais MySQL:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Eventra_DB;Uid=root;Pwd=sua_senha;"
  }
}
```

3. Execute:
```bash
dotnet ef database update
dotnet run
```

**API disponível em:** `https://localhost:5001` | **Swagger:** `https://localhost:5001/swagger`

### Frontend
```bash
cd frontend
npm install
npm run dev
```

---

##  API - Principais Endpoints

### Autenticação
```http
POST /api/users/register
POST /api/users/login
```

### Eventos
```http
GET  /api/events              # Listar (público)
POST /api/events              # Criar (Organizer/Admin)
PUT  /api/events/{id}         # Editar (dono ou Admin)
DELETE /api/events/{id}       # Deletar (dono ou Admin)
```

### Inscrições
```http
POST /api/registrations                    # Inscrever (Participant)
GET  /api/registrations/my-registrations   # Minhas inscrições
DELETE /api/registrations/{id}             # Cancelar
```

**Documentação completa:** Acesse o Swagger após rodar o backend

---

##  Arquitetura

```
Controllers → Services → Data (EF Core) → MySQL
```

**Padrões:** Repository Pattern • Dependency Injection • DTOs • Extension Methods

**Segurança:** JWT com roles • Validações de propriedade • Admin override • Transações ACID

---

##  Funcionalidades

**Usuários:** Registro, login JWT, perfil, exclusão com cascade  
**Eventos:** CRUD completo, 13 categorias, validação de datas  
**Inscrições:** Controle de vagas, prevenção de duplicatas, transações  
**Autorização:** Role-based (Admin/Organizer/Participant)

---

##  Roadmap

** Concluído:** Backend MVP completo  
** Em desenvolvimento:** Frontend Next.js  

---

