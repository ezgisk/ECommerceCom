# Online Shopping Platform - Multi-Layered ASP.NET Core Web API Project

## Project Overview

This project is an **Online Shopping Platform** built using a **multi-layered architecture** with ASP.NET Core Web API. The application uses **Entity Framework Core** for data access and is organized into three main layers:

1. **Presentation Layer** (API Layer) - Contains the controllers.
2. **Business Layer** (Service Layer) - Implements the business logic.
3. **Data Access Layer** (Repository Layer) - Manages database operations via Entity Framework.

This application incorporates several essential features like **authentication, authorization, logging, exception handling**, and more to create a functional, secure online shopping platform.

---

## Features

- **Multi-Layered Architecture**: Separated into 3 layers (Presentation, Business, and Data Access).
- **Entity Framework Core**: Used for data management and interactions with the database.
- **ASP.NET Core Identity / Custom Authentication**: Handles customer authentication and roles (e.g., "Customer", "Admin").
- **JWT Authentication**: Implements JSON Web Tokens for secure API access.
- **Model Validation**: Ensures proper input validation for models like User, Product, Order.
- **Middleware**: Logs requests and handles maintenance states.
- **Custom Action Filters**: Restrict access to certain APIs based on a time window.
- **Data Protection**: Secures user passwords using ASP.NET Core’s Data Protection API.
- **Global Exception Handling**: Catches and handles all errors globally.
- **Paging & Caching (Optional)**: Adds support for pagination and caching for enhanced performance.

---

## Solution Structure

### 1. **Presentation Layer (API Layer)**

- **Controllers**:
  - **UserController**: Handles user registration, login, profile management, etc.
  - **OrderController**: Manages customer orders, including viewing orders, placing new orders, and order details.
  - **ProductController**: Allows customers to browse available products, manage stock, and view details.
  
- **Endpoints**:
  - **User Endpoints**:
    - POST `/api/auth/register`: Register a new customer.
    - POST `/api/auth/login`: Customer login and JWT generation.
    - GET `/api/auth/{id}`: Retrieve a user by ID.
    - PUT `/api/auth/{id}`: Update user profile.
    - DELETE `/api/auth/{id}`: Delete a user.
  
  - **Order Endpoints**:
    - POST `/api/orders`: Create a new order.
    - GET `/api/orders/{id}`: Get order details by ID.
    - GET `/api/orders`: Get all orders for a customer.

  - **Product Endpoints**:
    - GET `/api/products`: List all products.
    - GET `/api/products/{id}`: View product details.
    - PUT `/api/products/{id}`: Update product details.
  
### 2. **Business Layer (Service Layer)**

- **Services**:
  - **UserService**: Handles all business logic related to user registration, authentication, and profile updates.
  - **OrderService**: Handles business logic for creating, updating, and viewing orders.
  - **ProductService**: Manages the business logic for product listing, stock updates, and managing product details.
  
- **Logic**:
  - User password encryption using **Data Protection**.
  - JWT token generation and validation for secure user access.
  - Business rules for ordering products, calculating total amount, etc.

### 3. **Data Access Layer (Repository Layer)**

- **Entities**:
  - **UserEntity**: Represents a customer.
  - **ProductEntity**: Represents a product in the store.
  - **OrderEntity**: Represents a customer order.
  - **OrderProductEntity**: Represents the many-to-many relationship between orders and products.
  
- **Repositories**:
  - **UserRepository**: CRUD operations for user entities.
  - **OrderRepository**: CRUD operations for order entities.
  - **ProductRepository**: CRUD operations for product entities.
  
- **Unit of Work**: Provides a single point of transaction management for all repositories.

---

## Database Schema

### **User Entity**
- `Id` (int, primary key)
- `FirstName` (string)
- `LastName` (string)
- `Email` (string, unique)
- `PhoneNumber` (string)
- `Password` (string, encrypted using Data Protection)
- `Role` (enum: "Customer", "Admin")

### **Product Entity**
- `Id` (int, primary key)
- `ProductName` (string)
- `Price` (decimal)
- `StockQuantity` (int)

### **Order Entity**
- `Id` (int, primary key)
- `OrderDate` (DateTime)
- `TotalAmount` (decimal)
- `CustomerId` (int, foreign key to User)
- `OrderProducts` (many-to-many relationship with Product)

### **OrderProduct Entity**
- `OrderId` (int, foreign key to Order)
- `ProductId` (int, foreign key to Product)
- `Quantity` (int)

---

## Key Features

### **Authentication & Authorization**

- **JWT Authentication**: Protects endpoints using JWT tokens.
- **Role-Based Authorization**: Differentiates access between **Admin** and **Customer** roles.
- **Custom Authentication**: Includes a custom service for handling user login, registration, and password encryption.

### **Middleware**

- **Request Logging Middleware**: Logs each incoming request with timestamp, user info, and URL.
- **Maintenance Middleware**: Handles application maintenance states, blocking access during maintenance windows.

### **Action Filters**

- **Time Window Access Control**: Custom action filter to restrict access to certain APIs within specified time windows.

### **Model Validation**

- **Product and User Validation**: Ensures proper input for product stock, customer email formats, etc.
- **Exception Handling**: Global error handling middleware that captures and logs exceptions.


