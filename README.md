# ETicaretAPI 🛒

<div align="center">
  
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-7.0-blue)
![EF Core](https://img.shields.io/badge/Entity%20Framework%20Core-7.0-brightgreen)
![License](https://img.shields.io/badge/License-MIT-yellow)

**A comprehensive e-commerce backend solution built with ASP.NET Core**
</div>

## 📋 Overview

ETicaretAPI is a robust e-commerce backend solution providing all the necessary functionality for running a modern e-commerce platform, including product management, user authentication, order processing, and more.

## 🚀 Technologies Used

<table>
  <tr>
    <td><b>Core Framework</b></td>
    <td>ASP.NET Core</td>
    <td>Primary web framework for building the API</td>
  </tr>
  <tr>
    <td><b>ORM</b></td>
    <td>Entity Framework Core</td>
    <td>Database operations and data access</td>
  </tr>
  <tr>
    <td><b>Authentication</b></td>
    <td>Identity Framework & JWT</td>
    <td>User authentication and authorization</td>
  </tr>
  <tr>
    <td><b>Real-time</b></td>
    <td>SignalR</td>
    <td>Real-time web functionality for notifications</td>
  </tr>
  <tr>
    <td><b>Object Mapping</b></td>
    <td>AutoMapper</td>
    <td>Mapping between models and DTOs</td>
  </tr>
  <tr>
    <td><b>API Documentation</b></td>
    <td>Swagger/OpenAPI</td>
    <td>Interactive API documentation</td>
  </tr>
  <tr>
    <td><b>Validation</b></td>
    <td>FluentValidation</td>
    <td>Input validation for requests</td>
  </tr>
  <tr>
    <td><b>Architecture Patterns</b></td>
    <td>CQRS & Repository</td>
    <td>Clean separation of concerns</td>
  </tr>
</table>

## 🏗️ Project Structure

The solution follows clean architecture principles:

```
ETicaretAPI/
├── ETicaretAPI.WebAPI        # API controllers and entry point
├── ETicaretAPI.Application   # Business logic and services
├── ETicaretAPI.Domain        # Domain entities and business rules
├── ETicaretAPI.Infrastructure # External service implementations
└── ETicaretAPI.Persistence   # Data access layer and repositories
```

## 🔧 Getting Started

### Prerequisites

- ✅ .NET 7.0 SDK
- ✅ SQL Server - PostgreSQL (or preferred database engine)
- ✅ Git (for cloning the repository)

### Installation

1️⃣ **Clone the repository**
```bash
git clone https://github.com/Trustedaid/ETicaretAPI.git
```

2️⃣ **Navigate to the project directory**
```bash
cd ETicaretAPI
```

3️⃣ **Restore NuGet packages**
```bash
dotnet restore
```

4️⃣ **Update database connection string** in `appsettings.json` located in the WebAPI project:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ETicaretDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

5️⃣ **Apply migrations** to create the database:
```bash
dotnet ef database update --project ETicaretAPI.Persistence --startup-project ETicaretAPI.WebAPI
```

6️⃣ **Run the application**:
```bash
dotnet run --project ETicaretAPI.WebAPI
```

7️⃣ **Access Swagger documentation** at:
```
https://localhost:5001/swagger
```

## ⚙️ Configuration Parameters

### JWT Authentication

Modify the JWT settings in `appsettings.json`:

```json
"JwtSettings": {
  "Secret": "YOUR_SECRET_KEY_HERE_MINIMUM_16_CHARACTERS",
  "Issuer": "ETicaretAPI",
  "Audience": "ETicaretClient",
  "DurationInMinutes": 60
}
```

### Email Service

Configure email service parameters for user notifications:

```json
"EmailSettings": {
  "SmtpServer": "smtp.example.com",
  "SmtpPort": 587,
  "SenderEmail": "notifications@yourstore.com",
  "SenderName": "Your Store",
  "Password": "YOUR_SMTP_PASSWORD"
}
```

### Payment Gateway

Set up payment gateway integration parameters:

```json
"PaymentGateway": {
  "ApiKey": "YOUR_PAYMENT_GATEWAY_API_KEY",
  "SecretKey": "YOUR_PAYMENT_GATEWAY_SECRET_KEY",
  "IsTestMode": true
}
```

## 🔌 API Controllers and Endpoints

### 📦 ProductsController

Manages product-related operations.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| GET | `/api/products` | Retrieves all products with pagination |
| GET | `/api/products/{id}` | Retrieves a specific product by ID |
| POST | `/api/products` | Creates a new product |
| PUT | `/api/products/{id}` | Updates an existing product |
| DELETE | `/api/products/{id}` | Deletes a product |
| GET | `/api/products/category/{categoryId}` | Retrieves products by category |
| GET | `/api/products/search` | Searches products by name or description |

### 👤 UsersController

Handles user account management.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| POST | `/api/users/register` | Registers a new user |
| POST | `/api/users/login` | Authenticates a user and returns JWT token |
| GET | `/api/users/profile` | Retrieves the current user's profile |
| PUT | `/api/users/profile` | Updates the current user's profile |
| POST | `/api/users/change-password` | Changes the user's password |
| POST | `/api/users/refresh-token` | Refreshes the JWT authentication token |

### 📁 CategoriesController

Manages product categories.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| GET | `/api/categories` | Retrieves all categories |
| GET | `/api/categories/{id}` | Retrieves a specific category by ID |
| POST | `/api/categories` | Creates a new category |
| PUT | `/api/categories/{id}` | Updates an existing category |
| DELETE | `/api/categories/{id}` | Deletes a category |

### 📋 OrdersController

Handles order processing.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| GET | `/api/orders` | Retrieves all orders for the current user |
| GET | `/api/orders/{id}` | Retrieves a specific order by ID |
| POST | `/api/orders` | Creates a new order |
| PUT | `/api/orders/{id}/status` | Updates the status of an order |
| GET | `/api/orders/admin` | Admin endpoint to view all orders (requires admin role) |

### 🛒 CartController

Manages shopping cart operations.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| GET | `/api/cart` | Retrieves the current user's cart |
| POST | `/api/cart/add` | Adds a product to the cart |
| PUT | `/api/cart/update` | Updates the quantity of a cart item |
| DELETE | `/api/cart/remove/{itemId}` | Removes an item from the cart |
| DELETE | `/api/cart/clear` | Clears the entire cart |

### 💳 PaymentsController

Handles payment processing.

| Method | Endpoint | Description |
|:------:|----------|-------------|
| POST | `/api/payments/process` | Processes a payment for an order |
| POST | `/api/payments/verify` | Verifies a payment transaction |
| POST | `/api/payments/callback` | Callback endpoint for payment gateway integration |

## 🔐 Authentication

The API uses JWT (JSON Web Token) based authentication. To access protected endpoints:

1. Obtain a JWT token by calling the login endpoint:
   ```
   POST /api/users/login
   ```

2. Include the token in the Authorization header for subsequent requests:
   ```
   Authorization: Bearer {your_jwt_token}
   ```

## 🔑 Role-Based Authorization

The API implements role-based access control with the following roles:

- **Customer** - Regular user with access to product browsing, cart management, and order placement
- **Admin** - Administrative user with access to all endpoints and operations
- **Manager** - Special role for handling order management and product inventory

## ⚠️ Error Handling

The API returns standardized error responses:

```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": [
    "Product name is required",
    "Price must be greater than zero"
  ]
}
```

## ✅ Data Validation

Input validation is implemented using FluentValidation. All request DTOs are validated before processing.

## 📝 Logging

The application uses structured logging with Serilog. Configure logging settings in `appsettings.json`:

```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Information",
    "Override": {
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "WriteTo": [
    {
      "Name": "Console"
    },
    {
      "Name": "File",
      "Args": {
        "path": "logs/log-.txt",
        "rollingInterval": "Day"
      }
    }
  ]
}
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -m 'Add new feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE.md file for details.
