# Sales Order Management System

A clean-architecture sales order management system built with Blazor Server, MudBlazor, and Microsoft SQL Server.

## Architecture

This application follows clean architecture principles with the following layers:

- **Domain Layer**: Contains domain entities, enums, and interfaces
- **Application Layer**: Contains DTOs, service interfaces, and business logic contracts
- **Infrastructure Layer**: Contains data access, repositories, and service implementations
- **Web Layer**: Contains the Blazor Server UI with MudBlazor components

## Features

- ✅ **Dashboard**: View statistics and recent orders
- ✅ **CRUD Operations**: Full Create, Read, Update, Delete operations for:
  - Sales Orders
  - Customers
  - Products
- ✅ **Authentication & Authorization**: ASP.NET Core Identity integration
- ✅ **Audit System**: Automatic tracking of entity changes
- ✅ **Clean UI**: Modern UI with MudBlazor components
- ✅ **SQL Server**: Database persistence with Entity Framework Core

## Prerequisites

- .NET 10.0 SDK
- Microsoft SQL Server (LocalDB or full SQL Server)
- Visual Studio 2022 or Visual Studio Code (optional)

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd SalesOrderDemo
```

### 2. Update Connection String

Update the connection string in `src/SalesOrderDemo.Web/appsettings.json` to point to your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SalesOrderDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Apply Database Migrations

```bash
cd src/SalesOrderDemo.Infrastructure
dotnet ef database update --startup-project ../SalesOrderDemo.Web
```

### 4. Run the Application

```bash
cd ../SalesOrderDemo.Web
dotnet run
```

The application will start and be available at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

### 5. Register a User

Navigate to `/Account/Register` to create your first user account.

## Project Structure

```
SalesOrderDemo/
├── src/
│   ├── SalesOrderDemo.Domain/          # Domain entities and interfaces
│   │   ├── Common/                     # Base entities
│   │   ├── Entities/                   # Domain entities
│   │   ├── Enums/                      # Enumerations
│   │   └── Interfaces/                 # Repository interfaces
│   ├── SalesOrderDemo.Application/     # Application logic
│   │   ├── DTOs/                       # Data Transfer Objects
│   │   └── Interfaces/                 # Service interfaces
│   ├── SalesOrderDemo.Infrastructure/  # Data access and services
│   │   ├── Data/                       # DbContext
│   │   ├── Identity/                   # Identity configuration
│   │   ├── Repositories/               # Repository implementations
│   │   └── Services/                   # Service implementations
│   └── SalesOrderDemo.Web/             # Blazor Server UI
│       ├── Components/                 # Blazor components
│       │   ├── Layout/                 # Layout components
│       │   └── Pages/                  # Page components
│       └── Program.cs                  # Application entry point
└── README.md
```

## Technologies Used

- **Backend**:
  - .NET 10.0
  - ASP.NET Core Blazor Server
  - Entity Framework Core 10.0
  - ASP.NET Core Identity
  - Microsoft SQL Server

- **Frontend**:
  - MudBlazor 8.15.0
  - Blazor Server (Interactive rendering)

## Key Features Implementation

### Authentication & Authorization
- ASP.NET Core Identity for user management
- Login, Register, and Logout pages
- Secure authentication cookies

### Audit System
- Automatic tracking of Created/Modified dates
- Audit log entity for tracking changes
- User-based audit trail

### Dashboard
- Real-time statistics
- Order status breakdown
- Recent orders list

### CRUD Operations
- Customers: Manage customer information
- Products: Manage product catalog with SKU, pricing, and inventory
- Sales Orders: Create and manage orders with line items

## Database Schema

The application uses the following main entities:

- **Customers**: Customer information (Name, Email, Phone, Address, etc.)
- **Products**: Product catalog (Name, SKU, Description, Price, Stock)
- **SalesOrders**: Order headers (OrderNumber, Date, Customer, Status, Total)
- **OrderItems**: Order line items (Product, Quantity, Price)
- **AuditLogs**: Audit trail for entity changes
- **AspNetUsers**: Identity users and roles

## Development

### Building the Solution

```bash
dotnet build
```

### Running Tests

```bash
dotnet test
```

### Creating New Migrations

```bash
cd src/SalesOrderDemo.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../SalesOrderDemo.Web
```

### Updating the Database

```bash
cd src/SalesOrderDemo.Infrastructure
dotnet ef database update --startup-project ../SalesOrderDemo.Web
```

## License

This project is licensed under the MIT License.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.