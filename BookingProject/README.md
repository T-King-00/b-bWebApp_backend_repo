# Bed and Breakfast Booking API

Backend API for a Bed and Breakfast booking project. The project is built with ASP.NET Core, Entity Framework Core, and SQLite.

## Current Code Context

The backend currently uses controllers, services, Entity Framework Core, and SQLite:

- `Program1.cs` configures controllers, Swagger/OpenAPI, CORS, SQLite, dependency injection, database migrations, and JSON options.
- `AppDbContext` defines database tables for hotels, rooms, customers, and bookings.
- `HotelService` handles hotel data access and includes rooms, beds, and prices.
- `RoomService` handles room data access, including room availability checks against confirmed bookings.
- `BookingService` handles booking lookup, create, update, and delete operations through Entity Framework Core.
- `BookingValidators` validates booking dates and duplicate booking IDs before saving booking changes.
- `BookingController` currently exposes booking lookup by ID. The other booking controller actions are still placeholders.
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
- Booking service methods for:
  - Get booking by ID
  - Add booking
  - Update booking
  - Delete booking
- Booking validation for invalid date ranges.
- Booking exceptions for duplicate booking IDs, missing bookings, invalid dates, and failed saves.
- Unit tests for booking add, update, delete, duplicate ID, and invalid date behavior.

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

### Bookings

```http
GET /Booking/{bookingId}
```

Returns one booking by ID.

Current behavior:

- Negative booking IDs throw `BookingNotFoundInDbException`.
- Missing booking IDs throw `BookingNotFoundInDbException`.
- Found bookings are returned with `200 OK`.

## Partly Finished

- `RoomService` has add, update, and delete methods, but the controller does not expose admin endpoints for these yet.
- `HotelService` has add, update, delete, and get-all methods, but only the single hotel details endpoint is currently exposed.
- `BookingService` has get, add, update, and delete methods, but `BookingController` only exposes get-by-id right now.
- Booking create, update, and delete controller endpoints still throw `NotImplementedException`.
- Room availability logic exists in `RoomService`, but `BookingController` availability endpoint is not implemented yet.

## Not Finished Yet

- Implement `POST /Booking` by calling `BookingService.Add`.
- Implement `PUT /Booking/{id}` by calling `BookingService.UpdateBooking`.
- Implement `DELETE /Booking/{id}` by calling `BookingService.Delete`.
- Check room availability endpoint in `BookingController`.
- Customer booking flow.
- Admin authentication/login.
- Payment handling.
- Frontend connection to all backend endpoints.
- Integration tests.
- Cleaner error handling with proper status codes such as `404 Not Found` instead of returning `500` for missing data.
- Remove hard-coded hotel ID `1` and support multiple hotel branches dynamically.
- Decide whether `/rooms` should return all rooms when dates are missing or keep `/allRooms` as the separate endpoint.

## What I Learned

- How to create an ASP.NET Core Web API with controllers and services.
- How Entity Framework Core maps C# models to database tables.
- How to use SQLite as a local development database.
- How to Linq to SQL to query database tables.
- How dependency injection connects controllers to services.
- How to seed database data from a JSON file using manual method.
- How to include related data with `Include` and `ThenInclude`.
- How to avoid JSON reference cycle problems with `ReferenceHandler.IgnoreCycles`.
- How to handle exceptions in controllers and services by implementing `IExceptionHandler`.
- How to use middleware and their importance.

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

1. Add request/response DTOs instead of returning EF entities directly.
2. Implement booking create, update, and delete endpoints in `BookingController`.
3. Replace hard-coded hotel ID `1`.
4. Add tests for room availability date overlap logic.
5. Improve controller error handling.
6. Connect frontend room search to `/rooms`.
