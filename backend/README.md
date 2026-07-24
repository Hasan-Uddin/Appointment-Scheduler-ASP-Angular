# Appointment-Scheduler Backend

An App similar to the [Calendly](https://calendly.com/) built with .NET 9, implementing Clean Architecture principles, CQRS pattern, and Domain-Driven Design (DDD).

## 🏗️ Architecture

This project follows **Clean Architecture** with clear separation of concerns across multiple layers:

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│            (Web.Api)                │
├─────────────────────────────────────┤
│       Application Layer             │
│     (Use Cases & Handlers)          │
├─────────────────────────────────────┤
│        Domain Layer                 │
│   (Entities & Business Logic)       │
└─────────────────────────────────────┘
         ↑
         │ Dependencies
         │
┌────────┴────────┐
│ Infrastructure  │
│ (EF Core, Auth) │
└─────────────────┘
```

### Key Architectural Patterns

- **Clean Architecture**: Dependency inversion with core business logic independent of external concerns
- **CQRS**: Command Query Responsibility Segregation for read/write operations
- **Domain Events**: Event-driven architecture for decoupled domain logic
- **Result Pattern**: Railway-oriented programming for error handling
- **Repository Pattern**: Data access abstraction via DbContext
- **Decorator Pattern**: Cross-cutting concerns (validation, logging) via Scrutor

## 🚀 Features

### Authentication & Authorization
- **JWT Bearer Token** authentication
- **Permission-based** authorization system
- Secure **password hashing** with BCrypt
- User registration and login
- Claims-based identity management

### Domain Features
- **User Management**: Registration, authentication, profile retrieval
- **Todo Management**: Full CRUD operations with priority levels
- **Domain Events**: Async event handling for domain state changes

### Technical Features
- **FluentValidation**: Request validation with the decorator pattern
- **Entity Framework Core**: SQLite database with migrations
- **Health Checks**: Application and database health monitoring
- **Structured Logging**: Serilog with Seq integration
- **Swagger/OpenAPI**: Interactive API documentation
- **Docker Support**: Multi-container deployment with docker-compose

## 🛠️ Technology Stack

- **Framework**: .NET 9.0
- **Database**: SQLite
- **ORM**: Entity Framework Core 9.0
- **Authentication**: JWT Bearer Tokens
- **Validation**: FluentValidation 12.0
- **Logging**: Serilog with Seq
- **Testing**: xUnit, NetArchTest
- **Containerization**: Docker & Docker Compose

## 📋 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQLite](https://sqlite.org/) (if running without Docker)

## 🚦 Getting Started

### Option 1: Docker

```powershell
docker-compose up --build
```

### Option 2: Local Development

2. **Update connection string** (if needed)
   
   Edit `src/Web.Api/appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "Database": "Data Source=appointment-scheduler.db"
   }
   ```

3. **Apply database migrations**
   ```powershell
   cd src/Web.Api
   dotnet ef database update
   ```

4. **Run the application**
   ```powershell
   dotnet run
   ```

5. **Access Swagger UI**
   
   Navigate to: `https://localhost:5001/swagger`

## 📁 Project Structure

```
AuthServer/
├── src/
│   ├── Domain/                    # Enterprise business rules
│   │   ├── Users/                 # User aggregate
│   │   └── Todos/                 # Todo aggregate
│   ├── Application/               # Application business rules
│   │   ├── Abstractions/          # Interfaces & contracts
│   │   ├── Users/                 # User use cases
│   │   └── Todos/                 # Todo use cases
│   ├── Infrastructure/            # External concerns
│   │   ├── Authentication/        # JWT & password hashing
│   │   ├── Authorization/         # Permission system
│   │   ├── Database/              # EF Core DbContext
│   │   └── DomainEvents/          # Event dispatcher
│   ├── SharedKernel/              # Shared primitives
│   │   ├── Entity.cs              # Base entity
│   │   ├── Result.cs              # Result pattern
│   │   └── Error.cs               # Error handling
│   └── Web.Api/                   # Presentation layer
│       ├── Endpoints/             # Minimal API endpoints
│       └── Middleware/            # HTTP pipeline
└── tests/
    └── ArchitectureTests/         # Architecture enforcement tests
```
### Health & Monitoring

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/health` | Application health check |

## 🧪 Testing

### Run Architecture Tests
```powershell
dotnet test tests/ArchitectureTests
```

Architecture tests enforce:
- Domain layer has no dependencies on Application, Infrastructure, or Presentation
- Application layer has no dependencies on Infrastructure or Presentation
- Infrastructure layer has no dependencies on Presentation


## 📊 Code Quality

This project maintains high code quality standards:

- ✅ **TreatWarningsAsErrors**: Enabled
- ✅ **Nullable Reference Types**: Enabled
- ✅ **SonarAnalyzer**: Static code analysis
- ✅ **Architecture Tests**: Layer dependency enforcement
- ✅ **Central Package Management**: Consistent versioning

## 🤝 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👥 Authors

**[Hasan Uddin](https://github.com/Hasan-Uddin)**

## 🙏 Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- CQRS pattern inspiration from various enterprise implementations

---

**Built with ❤️ using .NET 9**
