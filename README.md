# Library Management App - Backend API

A RESTful API for managing a lending library, built with C# .NET 8. This backend service handles authentication, authorization, and all core operations for three distinct user roles: Readers, Librarians, and Admins.

## 🚀 Technologies

*   **Framework:** .NET 8
*   **Architecture:** Clean Architecture / Layered Architecture
*   **Database:** SQL Server with Entity Framework Core 8
*   **Authentication & Authorization:** ASP.NET Core Identity with JWT (JSON Web Tokens)
*   **API Documentation:** Swagger/OpenAPI (Swashbuckle)
*   **Object-Relational Mapping:** Entity Framework Core 8
*   **AutoMapper:** For object-object mapping
*   **Dependency Injection:** Built-in .NET IoC container
*   **Security:** Password hashing, role-based authorization


## ⚙️ Prerequisites & Installation

### Requirements:
1.  **.NET 8 SDK:** [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
2.  **SQL Server:** SQL Server 2022 Express or higher [Download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
3.  **Visual Studio 2022+** or **Visual Studio Code** with C# extensions
4.  **SQL Server Management Studio (SSMS)** or Azure Data Studio (optional)

### Setup Steps:

```bash
# 1. Clone the repository
git clone https://github.com/Eirini-Triantafyllou/LibraryManagementApp-backend.git
cd LibraryManagementApp-backend

# 2. Configure the database connection
# Open the solution in Visual Studio or your preferred IDE
# Navigate to: LibraryManagementApp.API/appsettings.json
# Update the ConnectionStrings.DefaultConnection to match your SQL Server instance:
# "Server=(localdb)\\mssqllocaldb;Database=LibraryManagementDB;Trusted_Connection=True;"
# or use: "Server=localhost;Database=LibraryManagementDB;Trusted_Connection=True;"

# 3. Apply database migrations
#   Using Package Manager Console (in Visual Studio):
#   Set Default project to: LibraryManagementApp.Infrastructure
#   Run: Update-Database

# 4. Run the application
# In Visual Studio: Press F5 or Ctrl+F5
# Using .NET CLI:
dotnet run --project LibraryManagementApp.API
```

## 🌐 API Access & Documentation

Once the application is running:

The API will be accessible at:

*   **HTTPS:** `https://localhost:5001`
*   **HTTP:** `http://localhost:5000`

Interactive API documentation (Swagger UI) will be available at:

*   `https://localhost:5001/swagger/index.html` 
