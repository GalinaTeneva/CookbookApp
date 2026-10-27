# 🚀 Cookbook App

> Web application for creating and managing personal recipes.

![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-8.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

---

## 📖 About the Project

CookbookApp helps users keep their favorite recipes organized in a personal digital cookbook. Registered users can create recipes, specify ingredients and quantities, write cooking instructions, and organize recipes by category.

The application uses a layered architecture to separate the web interface, business logic, data access, and domain models. Recipe management is restricted to authenticated users, with ownership checks protecting access to individual recipes.

---

## 🛠️ Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 8.0      | Web framework                    |
| Entity Framework Core | 8.0      | ORM / Database access            |
| SQL Server            | -        | Database                         |
| Bootstrap             | 5.3      | Frontend styling                 |
| Razor Pages / Views   | -        | Server-side HTML rendering       |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Apply database migrations

```bash
dotnet ef database update
```

### 4. Run the application

```bash
dotnet run
```

The app will be available at `https://localhost:5001` or `http://localhost:5000`.

---

## 📁 Project Structure

```
| Project | Responsibility |
| --- | --- |
| `CookbookApp` | Web application, controllers, views, Identity pages, and static assets |
| `Cookbook.Common` | Shared validation definitions |
| `Cookbook.Data.Models` | Database entities |
| `Cookbook.Data` | EF Core database context, entity configuration, and migrations |
| `Cookbook.Services.Core` | Application services and service contracts |
| `Cookbook.Services.Data` | Service request and response models |
| `Cookbook.Web.ViewModels` | View models and form input models |
```

---

## ✨ Features

- [ ] User registration and login (ASP.NET Identity)
- [ ] CRUD operations for Recipe
- [ ] RESTful API endpoints
- [ ] Input validation (server-side & client-side)
- [ ] Responsive UI with Bootstrap

---

## 💻 Usage

```
1. Open `https://localhost:7254` and register at `/Identity/Account/Register`, or log in at `/Identity/Account/Login`.
2. Visit `/Recipes` for your cookbook overview, including recipe totals, category counts, and your latest recipe.
3. Open `/Recipes/MyRecipes` to browse your collection, or `/Recipes/Add` to create a recipe.
4. Enter a title, select a category, and enter ingredients and cooking instruction steps. Description and image URL are optional.
5. Use the recipe actions to view details, edit, or delete your own recipes. Other users' recipes are not accessible from your account.
```


## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDbName;Trusted_Connection=True;"
}
```

To create and seed the database:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-connection-string-here"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## 🤝 Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a new branch: `git checkout -b feature/your-feature-name`
3. Commit your changes: `git commit -m "Add some feature"`
4. Push to the branch: `git push origin feature/your-feature-name`
5. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

*Built as part of the **ASP.NET Fundamentals** course.*
