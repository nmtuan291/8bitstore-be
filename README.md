# 8bitstore-be

## Overview
8bitstore-be is the backend for an e-commerce application built with ASP.NET Core 8. It provides user authentication, product catalog, cart, wishlist, orders, reviews, payments (VNPay), email notifications, and integrations for caching and search.

## Features
- Authentication and authorization with ASP.NET Core Identity (cookie-based)
- Product catalog with filtering, detail view, and search suggestions
- Shopping cart and wishlist management
- Order placement and review system
- VNPay payment gateway integration
- Email notifications via SMTP (MailKit/MimeKit)
- PostgreSQL with Entity Framework Core (code-first, auto-migrations on startup)
- Redis caching
- Swagger/OpenAPI docs in Development environment

## Tech Stack
- ASP.NET Core 8 (Web API)
- Entity Framework Core with PostgreSQL (Npgsql)
- ASP.NET Core Identity
- Redis (StackExchange.Redis)
- Swagger (Swashbuckle)
- MailKit/MimeKit
- Docker and Docker Compose

## Project Structure
- `8bitstore-be/Program.cs`: application startup, DI, CORS, middleware, EF Core, Identity, Redis
- `8bitstore-be/Data/_8bitstoreContext.cs`: EF Core `DbContext` (code-first)
- `8bitstore-be/Controllers`: API endpoints (users, products, cart, wishlist, orders, reviews, payments)
- `8bitstore-be/Services` and `8bitstore-be/Interfaces`: business logic and contracts
- `8bitstore-be/Models`: domain models
- `8bitstore-be/Migrations`: EF Core migrations
- `8bitstore-be/Dockerfile`, `8bitstore-be/docker-compose.yml`: containerization

## Prerequisites
- .NET 8 SDK
- Docker and Docker Compose (optional, recommended for full stack)

## Running Locally (without Docker)
1) Restore dependencies
```
dotnet restore
```
2) Run the API (from the project directory)
```
cd 8bitstore-be
DOTNET_ENVIRONMENT=Development dotnet run
```
The API defaults to `http://localhost:8080` in Production (via `PORT`) and standard Kestrel dev ports otherwise. Swagger UI is enabled in Development at `/swagger`.

EF Core migrations are applied automatically on startup. To manage migrations manually:
```
cd 8bitstore-be
# Create a new migration
dotnet ef migrations add <MigrationName>
# Apply migrations
dotnet ef database update
```

Update passwords and secrets in the compose file or via environment variables before use.

### Build and Run with Docker (API only)
```
cd 8bitstore-be
docker build -t 8bitstore-be .
docker run -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__WebApiDatabase="Host=host.docker.internal;Database=8bitstore;Username=postgres;Password=your_password" \
  -e Redis__ConnectionString="host.docker.internal:6379" \
  8bitstore-be
```

## API Documentation
- Swagger/OpenAPI is enabled in Development: navigate to `/swagger` when running locally or via Compose.
- Controllers include: `UserController`, `ProductController`, `CartController`, `WishlistController`, `OrderController`, `ReviewController`, `PaymentController`.

### Sample Endpoints
- `POST /api/User/signup`
- `POST /api/User/login`
- `GET /api/User/get-user` (authorized)
- `PUT /api/User/address/update` (authorized)
- `GET /api/Product/get-products`
- `GET /api/Product/get-all`
- `GET /api/Product/get-product?productId=...`
- `POST /api/Product`
- `GET /api/Product/get-suggestion?query=...`

Refer to Swagger for the complete, up-to-date list and schemas.

## Security Notes
- Do not commit secrets. Override all sensitive values via environment variables in local and production environments.
- CORS origins are restricted by default; update as needed for your frontends.
- Authentication is configured via ASP.NET Core Identity cookies.

## Deployment Notes
- In Production, the app respects the `PORT` environment variable and binds to `http://0.0.0.0:<PORT>` (default `8080`).
- When behind a proxy (e.g., Render), forwarded headers are enabled in Production.
- Enable HTTPS at the platform/proxy level; the app only forces HTTPS redirection in Production.

## Troubleshooting
- Migration failures: verify the `ConnectionStrings__WebApiDatabase` value and database availability; run `dotnet ef database update` manually if needed.
- Port conflicts: adjust exposed ports in Docker or use different local ports.
- CORS errors: update the `AllowFrontend` policy in `Program.cs` to include your origin(s).