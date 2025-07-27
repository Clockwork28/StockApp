# 📈 StockApp

ASP.NET Core Web API for managing stock portfolios, user comments, and integration with external market data API (FMP).  
This is an educational project demonstrating clean architecture, design patterns, database integration, and JWT-based authentication.

---

## 🚀 Features

-  JWT-based user registration and login
-  CRUD operations on stocks (add/edit/delete)
-  Personal investment portfolio management
-  Comments system per stock (CRUD + user assignment)
-  Filtering and sorting stocks by name and symbol
-  Integration with Financial Modeling Prep (FMP) API
-  Role-based authorization (User / Admin)

---

## 🛠️ Technologies

- ASP.NET Core 8 Web API
- Entity Framework Core
- Identity + JWT
- Swagger
- SQL Server
- FMP API (external data)

---

## 🧩 Project Structure

```bash
StockApp/
├── Controllers/          # API endpoints
├── Data/                 # ApplicationDbContext
├── DTOs/                 # Transfer objects (Stock, Comment, Account)
├── Extensions/           # ClaimsPrincipal helper
├── Helpers/              # Query parameters
├── Interfaces/           # Repository/service contracts
├── Mappers/              # DTO <-> Entity mappers
├── Migrations/           # EF Core DB migrations
├── Models/               # EF entities (User, Stock, Comment, Portfolio)
├── Repositories/         # Repository implementations
├── Services/             # Token and FMP integration
└── Program.cs            # Application configuration and DI
```

---

## ▶️ How to Run

> Requirements: .NET 8 SDK, SQL Server

1. Clone the repository  
   `git clone https://github.com/Clockwork28/StockApp.git`

2. Add your FMP API key to `appsettings.json`:
   ```json
   "Fmp": {
     "FmpKey": "your_api_key_here"
   }
   ```

3. Run migrations:
   ```bash
   dotnet ef database update
   ```

4. Start the app:  
   `dotnet run`

5. Visit `https://localhost:{port}/swagger` to explore the API

---

## 🧪 Example Endpoints

- `POST /api/account/register` – user registration
- `POST /api/account/login` – user login with JWT
- `GET /api/stock` – list of stocks (with filtering)
- `POST /api/portfolio/{symbol}` – add stock to portfolio
- `GET /api/comment/{symbol}` – get comments for a stock

---

## 👤 Author

Krzysztof Sumera  
[github.com/Clockwork28](https://github.com/Clockwork28)

---

## 📄 License

MIT – free to use for learning
