Event Management System

📌 Overview

The Event Management System is a .NET 8-based web application designed to streamline event creation and participant registration. It implements CQRS (Command Query Responsibility Segregation), MediatR, FluentValidation, and Repository Pattern to ensure clean architecture and maintainability.

📌 Tech Stack

Backend: ASP.NET Core 8, MediatR, FluentValidation, AutoMapper

Database: SQL Server (or any relational DB)

Authentication: ASP.NET Identity with JWT Authentication

Infrastructure: Entity Framework Core

Architecture: CQRS, Domain-Driven Design (DDD), Repository Pattern

📌 Features

1️⃣ Authentication

Event creators/Admin must log in (ASP.NET Identity + JWT)

Participants can register without login

2️⃣ Event Management (CQRS)

Create an Event (POST /api/events/create) (Command)

View All Events (GET /api/events) (Query)

3️⃣ Registration Management (CQRS)

Register for Event (POST /api/registrations/register) (Command)

View Registrations (GET /api/registrations/{eventId}/registrations) (Query)

4️⃣ Middleware

Exception Handling

Performance Logging

5️⃣ API Documentation

Integrated with Swagger & OpenAPI

📌 Architecture & Design Approaches

1️⃣ CQRS (Command Query Responsibility Segregation)

Commands (for POST, PUT, DELETE) are handled via MediatR

Queries (for GET requests) go through query handlers

2️⃣ Domain-Driven Design (DDD)

Entities: Event, Registration

Repositories: IEventRepository, IRegistrationRepository

3️⃣ Repository Pattern

The Repository Pattern is used to separate business logic from database interactions, ensuring that:

The domain layer remains independent of the data access layer.

Data access operations are abstracted via repository interfaces (IEventRepository, IRegistrationRepository).

Query optimizations (e.g., AsNoTracking() for read operations) are applied to improve performance.

4️⃣ Dependency Injection

Services and repositories are injected via DI container

5️⃣ FluentValidation

Command & Query validations handled via FluentValidation

📌 API Endpoints

1️⃣ Authentication

POST /api/auth/login (Login)

2️⃣ Event Management

POST /api/events/create (Create an event)

GET /api/events (Get all events)

3️⃣ Registration Management

POST /api/registrations/register (Register for event)

GET /api/registrations/{eventId}/registrations (Get all registrations)

📌 Setup Instructions

1️⃣ Clone the Repository

git clone https://github.com/write2dipanwita/EventManagementSystem.git
cd EventManagementSystem

2️⃣ Install Dependencies

dotnet restore

3️⃣ Run Migrations & Update Database

dotnet ef migrations add InitialCreate
dotnet ef database update

4️⃣ Run the Application

dotnet run --project EventManagementSystem.WebAPI

📌 Testing

Run unit tests with:

dotnet test

📌 Future Improvements

Add email notifications for registrations

Add event reminders & calendar integration

🚀 Developed by: Dipanwita Roy🔗 GitHub: EventManagement Repo

