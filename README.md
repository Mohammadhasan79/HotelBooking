<div align="center">

# 🏨 HotelBooking

**A microservices-based hotel reservation platform built with .NET 9, Clean Architecture, JWT Auth, RabbitMQ event-driven messaging, and a YARP API Gateway — fully containerized with Docker Compose.**

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Event--Driven-FF6600?logo=rabbitmq&logoColor=white)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![YARP](https://img.shields.io/badge/Gateway-YARP-512BD4)
![License](https://img.shields.io/badge/License-MIT-green)

</div>

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Services](#-services)
- [Event-Driven Flow](#-event-driven-flow)
- [Tech Stack](#-tech-stack)
- [Getting Started](#-getting-started)
- [Accessing the API](#-accessing-the-api)
- [Authentication & Roles](#-authentication--roles)
- [Configuration](#-configuration)
- [Database Migrations](#-database-migrations)
- [Project Structure](#-project-structure)
- [API Walkthrough](#-api-walkthrough)
- [Connecting a Frontend](#-connecting-a-frontend)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## 🔎 Overview

HotelBooking is a sample **microservices architecture** project demonstrating how to design, build, and run a distributed .NET system end-to-end:

- 6 independently deployable services, each following **Clean Architecture** (Api / Application / Domain / Infrastructure)
- **JWT-based authentication & role-based authorization** (Admin / User)
- **Synchronous** service-to-service communication via HTTP (for validation/lookups)
- **Asynchronous** service-to-service communication via **RabbitMQ** (for domain events)
- A single **API Gateway** (YARP) as the unified entry point
- One **SQL Server** instance with a separate database per service
- Fully reproducible local environment via **Docker Compose** — one command, zero manual setup

---

## 🏗 Architecture

```
                          ┌─────────────────────┐
                          │     API Gateway       │
                          │   (YARP · :7068)       │
                          └──────────┬────────────┘
                                      │
        ┌───────────┬────────────────┼────────────────┬───────────┐
        │            │                │                │           │
   ┌────▼───┐   ┌────▼───┐      ┌─────▼────┐     ┌─────▼────┐ ┌────▼────┐
   │Identity│   │ Hotel  │      │   Room    │     │ Booking  │ │ Payment │
   │ :7001  │   │ :7002  │      │  :7003    │     │  :7004   │ │  :7005  │
   └───┬────┘   └───┬────┘      └────┬──────┘     └────┬─────┘ └────┬────┘
       │            │     HTTP        │                │            │
       │            └─────(exists?)───┘                │            │
       │                              └────(exists? price?)──────────┤
       │                                                              │
       │                       RabbitMQ Events                       │
       │              booking-created → payment-completed            │
       └──────────────────────────┬──────────────────────────────────┘
                                    │
                          ┌─────────▼─────────┐
                          │     SQL Server      │
                          │ (1 DB per service)   │
                          └─────────────────────┘
```

Each service follows the same internal layering:

```
ServiceName/
├── ServiceName.Api              → Controllers, Program.cs, DI composition root
├── ServiceName.Application       → DTOs, Service Interfaces, Validators, Mappings
├── ServiceName.Domain             → Entities, Enums
└── ServiceName.Infrastructure     → EF Core, Repositories, HTTP Clients, RabbitMQ
```

A shared library, **`Shared.Contracts`**, defines event contracts (`BookingCreatedEvent`, `PaymentCompletedEvent`) used by both publishers and consumers.

---

## 🧱 Services

| # | Service | Port | Database | Responsibility |
|---|---|---|---|---|
| 1 | **ApiGateway** | `7068` | — | Routes all incoming traffic to the correct service (YARP reverse proxy) |
| 2 | **IdentityService** | `7001` | `HotelIdentityDb` | User registration/login, JWT issuance, role seeding |
| 3 | **HotelService** | `7002` | `HotelServiceDb` | CRUD operations for hotels |
| 4 | **RoomService** | `7003` | `RoomServiceDb` | CRUD for rooms; validates hotel existence via HotelService |
| 5 | **BookingService** | `7004` | `BookingServiceDb` | Creates bookings; validates rooms via RoomService; publishes `booking-created` |
| 6 | **PaymentService** | `7005` | `PaymentServiceDb` | Consumes `booking-created`; processes payments; publishes `payment-completed` |

---

## 🔄 Event-Driven Flow

```
BookingService                RabbitMQ                 PaymentService
     │                                                        │
     │── 1. Create booking ─────────────────────────────────►│
     │   publish "booking-created"                           │
     │                                                        │
     │                                       2. Consume event │
     │                                  Create Payment(Pending)
     │                                                        │
     │◄── 4. Consume "payment-completed" ─── 3. Mark paid ────┤
     │   Confirm booking                  publish "payment-completed"
     ▼                                                        ▼
```

This decouples Booking and Payment — each reacts to events independently without direct synchronous dependency at runtime.

---

## 🛠 Tech Stack

| Category | Technology |
|---|---|
| Framework | .NET 9 / ASP.NET Core Web API |
| ORM | Entity Framework Core (SQL Server provider) |
| Auth | ASP.NET Core Identity + JWT Bearer |
| API Gateway | YARP (Yet Another Reverse Proxy) |
| Messaging | RabbitMQ |
| Mapping | AutoMapper |
| Validation | FluentValidation |
| API Docs | Swagger / Swashbuckle |
| Containerization | Docker & Docker Compose |
| Database | Microsoft SQL Server 2022 |

---

## 🚀 Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — installed and running

> No need to install .NET SDK, SQL Server, or RabbitMQ locally — everything runs inside containers.

### Run

```bash
git clone https://github.com/Mohammadhasan79/HotelBooking.git
cd HotelBooking
docker compose up --build
```

> ⏳ First run downloads base images and builds all 6 services — can take **10–30 minutes** depending on your connection. Subsequent runs are much faster due to Docker layer caching.

Wait until each service logs `Now listening on: http://[::]:8080` with no errors.

### Verify

```bash
docker ps
```

Expected: **8 running containers** — `sqlserver`, `rabbitmq`, and the 6 application services.

### Stop

```bash
docker compose down
```

---

## 🌐 Accessing the API

### Through the Gateway (recommended)

Base URL: **`http://localhost:7068`**

| Route prefix | Forwarded to |
|---|---|
| `/identity/*` | IdentityService |
| `/hotels/*` | HotelService |
| `/rooms/*` | RoomService |
| `/bookings/*` | BookingService |
| `/payments/*` | PaymentService |

```http
POST http://localhost:7068/identity/api/auth/login
GET  http://localhost:7068/hotels/api/hotels
POST http://localhost:7068/bookings/api/bookings
```

### Swagger (per service)

> The Gateway does not proxy Swagger UI — access each service's Swagger directly:

| Service | URL |
|---|---|
| Identity | http://localhost:7001/swagger |
| Hotel | http://localhost:7002/swagger |
| Room | http://localhost:7003/swagger |
| Booking | http://localhost:7004/swagger |
| Payment | http://localhost:7005/swagger |

### RabbitMQ Management

```
http://localhost:15672   (guest / guest)
```

---

## 🔑 Authentication & Roles

Authentication is handled by **IdentityService** using **ASP.NET Core Identity** + **JWT**. Two roles exist: **Admin** and **User**. Roles and a default admin account are seeded automatically on first run.

**Flow:**
1. `POST /identity/api/auth/register` or use the seeded admin
2. `POST /identity/api/auth/login` → returns a JWT
3. Send `Authorization: Bearer <token>` on protected endpoints

**Authorization matrix:**

| Endpoint | Access |
|---|---|
| `GET /hotels`, `GET /hotels/{id}` | Public |
| `POST/PUT/DELETE /hotels` | **Admin** |
| `GET /rooms`, `GET /rooms/{id}`, `GET /rooms/hotel/{hotelId}` | Public |
| `POST/PUT/DELETE /rooms` | **Admin** |
| `POST /bookings` | Authenticated user |
| `GET /bookings` (all) | **Admin** |
| `GET /bookings/{id}`, `GET /bookings/my`, `DELETE /bookings/{id}/cancel` | Authenticated user |
| `POST /payments/{id}/complete` | Authenticated user |

---

## ⚙ Configuration

Each service has an `appsettings.json` with **local defaults**. Docker Compose overrides the following via environment variables:

| Variable | Purpose |
|---|---|
| `ConnectionStrings__DefaultConnection` | Points to the `sqlserver` container |
| `RabbitMQ__HostName` | Points Booking/Payment to the `rabbitmq` container |
| `ExternalServices__HotelServiceBaseUrl` | RoomService → HotelService |
| `ExternalServices__RoomServiceBaseUrl` | BookingService → RoomService |
| `ASPNETCORE_ENVIRONMENT` | `Development`, to enable Swagger |

> ⚠️ **Security note:** The JWT secret and SQL `sa` password are hardcoded for local/demo convenience. Replace these with environment-injected secrets before any real deployment.

---

## 🗄 Database Migrations

Every service calls `dbContext.Database.Migrate()` on startup — databases and schemas are created automatically the first time the stack runs. Migrations are idempotent, so repeated runs are safe. **No manual `dotnet ef database update` is required.**

---

## 📁 Project Structure

```
HotelBooking/
├── ApiGateway/                   # YARP reverse proxy
├── Services/
│   ├── IdentityService/
│   ├── HotelService/
│   ├── RoomService/
│   ├── BookingService/
│   └── PaymentService/
├── Shared.Contracts/              # Shared event DTOs
├── docker-compose.yml
├── HotelBooking.sln
└── README.md
```

---

## 🧪 API Walkthrough

```http
### 1. Login
POST http://localhost:7068/identity/api/auth/login
{ "email": "admin@admin.com", "password": "..." }

### 2. Create a hotel (Admin)
POST http://localhost:7068/hotels/api/hotels
Authorization: Bearer <admin_token>

### 3. Create a room (Admin)
POST http://localhost:7068/rooms/api/rooms
Authorization: Bearer <admin_token>

### 4. Create a booking
POST http://localhost:7068/bookings/api/bookings
Authorization: Bearer <user_token>

### 5. Complete payment
POST http://localhost:7068/payments/api/payments/{paymentId}/complete
Authorization: Bearer <user_token>

### 6. Check booking status
GET http://localhost:7068/bookings/api/bookings/my
Authorization: Bearer <user_token>
```

---

## 🧩 Connecting a Frontend

- **Base URL:** `http://localhost:7068`
- Prefix every request with the relevant service route (`/identity`, `/hotels`, `/rooms`, `/bookings`, `/payments`)
- If your frontend runs on a different origin, enable CORS in `ApiGateway/Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapReverseProxy();
```

---

## 🗺 Roadmap

- [ ] Externalize secrets (JWT key, SQL password) via environment/secret manager
- [ ] Add `HEALTHCHECK` and `/health` endpoints for app services
- [ ] Centralized structured logging (Seq / ELK)
- [ ] API versioning
- [ ] Integration test suite per service
- [ ] CI/CD pipeline (GitHub Actions) for build & image publishing

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
