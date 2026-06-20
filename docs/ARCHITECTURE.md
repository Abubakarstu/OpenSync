# OpenSync Architecture

## Overview

OpenSync is a general-purpose real-time data synchronization library built on .NET 8 using Clean Architecture principles. It can run as a standalone microservice or be embedded into any ASP.NET Core application.

## Architecture Layers

### Core (Domain)
- Zero dependencies
- Contains entities, value objects, enums, domain events, exceptions, and interfaces
- Business rules are enforced in entity methods

### Application (Use Cases)
- Depends only on Core
- Implements CQRS pattern with MediatR commands/queries
- FluentValidation for input validation
- Pipeline behaviors for cross-cutting concerns

### Infrastructure
- Implements interfaces defined in Core/Application
- Entity Framework Core for persistence (PostgreSQL, SQLite, SQL Server)
- WebSocket/SSE/Long-polling transports
- In-process and Redis backplanes for multi-instance scaling
- JWT token service, rate limiting, TTL expiry

### API (Presentation)
- ASP.NET Core controllers
- Swagger/OpenAPI documentation
- Middleware for auth, rate limiting, correlation IDs, error handling
- WebSocket endpoint at `/ws`

## Real-Time Event Flow

1. Client sends command via REST API or WebSocket message
2. Application handler processes the command
3. Domain events are raised on entity changes
4. DomainEventInterceptor captures events on SaveChanges
5. Events are published via IEventPublisher
6. EventDispatcher maps events to subscriber connections
7. FanOutStrategy sends serialized messages to all subscribed transports
8. Multi-instance setup also publishes via IBackplane (Redis pub/sub)

## Data Flow

Client -> REST/WS -> Controller -> MediatR -> Handler -> Repository -> EF Core -> Database
                                         |
                                    Domain Event
                                         |
                              EventPublisher -> EventDispatcher -> FanOut -> Transports -> Clients
