# Bed and Breakfast Booking API

Backend API for a Bed and Breakfast booking project. The project is built with ASP.NET Core, Entity Framework Core, and SQLite.

## Current Code Context

The backend currently uses controllers, services, Entity Framework Core, and SQLite:

- `Program1.cs` configures controllers, Swagger/OpenAPI, CORS, SQLite, dependency injection, database migrations, and JSON options.
- `AppDbContext` defines database tables for hotels, rooms, customers, and bookings.
- `HotelService` handles hotel data access and includes rooms, beds, and prices.
- `RoomService` handles room data access, including room availability checks against confirmed bookings.
- `DbSeeder` loads initial hotel, room, bed, and price data from `Database/SeedingData.json`.

## Finished

- ASP.NET Core Web API project setup.
- SQLite database connection using Entity Framework Core.
- EF Core migrations configured and applied on app startup.
- Database seeding from JSON.
- Hotel model and room model connected through EF Core relationships.
- Room model includes:
  - Room type
  - Room size
  - Beds
  - Price
  - Hotel relationship
- Hotel details endpoint.
- Room list endpoint.
- Room details by ID endpoint.
- Room availability filtering using check-in and check-out dates.
- Date string parsing from query parameters into `DateOnly`.
- Date validation for invalid format and invalid check-out date.
- JSON configuration to avoid reference cycles.
- Swagger UI enabled in development.

## Available Endpoints

### Hotel

```http
GET /
```

Returns the current hotel branch with rooms, beds, and prices.

### Rooms

```http
GET /allRooms
```

Returns all rooms for the current hotel branch.

```http
GET /rooms?checkInDate=2026-06-20&checkOutDate=2026-06-22
```

Returns rooms available between the given dates.

Dates must use this format:

```text
yyyy-MM-dd
```

Example:

```text
2026-06-20
```

```http
GET /rooms/{id}
```

Returns one room by ID.

## Partly Finished

- `RoomService` has add, update, and delete methods, but the controller does not expose admin endpoints for these yet.
- `HotelService` has add, update, delete, and get-all methods, but only the single hotel details endpoint is currently exposed.
- Booking models exist and are connected to rooms and customers, but booking workflows are not finished.
- Room availability logic exists in `RoomService`, but booking creation is not implemented yet.

## Not Finished Yet

- Create booking endpoint.
- Get booking by ID endpoint.
- Update booking endpoint.
- Delete booking endpoint.
- Check room availability endpoint in `BookingController`.
- Customer booking flow.
- Admin authentication/login.
- Payment handling.
- Frontend connection to all backend endpoints.
- Unit tests and integration tests.
- Cleaner error handling with proper status codes such as `404 Not Found` instead of returning `500` for missing data.
- Remove hard-coded hotel ID `1` and support multiple hotel branches dynamically.
- Decide whether `/rooms` should return all rooms when dates are missing or keep `/allRooms` as the separate endpoint.

## What I Learned

- How to create an ASP.NET Core Web API with controllers and services.
- How dependency injection connects controllers to services.
- How Entity Framework Core maps C# models to database tables.
- How to use SQLite as a local development database.
- How to run EF Core migrations automatically when the app starts.
- How to seed database data from a JSON file.
- How to include related data with `Include` and `ThenInclude`.
- How to avoid JSON reference cycle problems with `ReferenceHandler.IgnoreCycles`.
- How to parse incoming query string dates into `DateOnly`.
- How to validate API input and return `BadRequest`.
- How room availability checks work by finding overlapping confirmed bookings.

## Important Current Issues

### SQLite migration lock

If the app prints repeated logs like this:

```text
Acquiring an exclusive lock for migration application
INSERT OR IGNORE INTO "__EFMigrationsLock"
```

it means EF Core is waiting for the SQLite migration lock.

Common causes:

- Another instance of the app is already running.
- The app stopped during migration and left a stale lock.

Fix:

- Stop the running `BookingProject` process.
- If needed, clear the `__EFMigrationsLock` table from the SQLite database.

### Build file locked

If build fails with:

```text
BookingProject.exe is being used by another process
```

stop the running backend app and build again.

## Run The Project

From the backend folder:

```powershell
dotnet build .\BookingProject\BookingProject.csproj
```

Then run:

```powershell
dotnet run --project .\BookingProject\BookingProject.csproj
```

In development, Swagger UI should be available after the app starts.

## Next Steps

1. Finish booking CRUD endpoints.
2. Add request/response DTOs instead of returning EF entities directly.
3. Replace hard-coded hotel ID `1`.
4. Add tests for room availability date overlap logic.
5. Improve controller error handling.
6. Connect frontend room search to `/rooms`.
