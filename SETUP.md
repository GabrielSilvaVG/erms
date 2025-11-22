# Backend

```bash
cd backend
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

API rodando em: https://localhost:5001 (ou http://localhost:5000)
Swagger: https://localhost:5001/swagger

# Frontend

```bash
cd frontend
npm install
npm run dev
```

App rodando em: http://localhost:3000
