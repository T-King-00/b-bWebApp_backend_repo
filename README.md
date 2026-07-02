# Bed and Breakfast Booking API

Backend API for a Bed and Breakfast booking project. The project is built with ASP.NET Core, Entity Framework Core, and SQLite.

## Current Code Context

The backend currently uses controllers, services, Entity Framework Core, SQLite, validation rules, and a global exception handler.

- `Program1.cs` configures controllers, Swagger/OpenAPI, CORS, SQLite, dependency injection, database migrations, global exception handling, and JSON options.
- `AppDbContext` defines database tables for hotels, rooms, customers, and bookings.
- `HotelService` handles hotel lookup and includes rooms, beds, and prices.
- `RoomService` handles room lookup, room details, total price calculation, and availability filtering against confirmed bookings.
- `CustomerService` handles customer lookup, creation, update, and delete logic.
- `BookingService` handles booking lookup, creation, update, deletion, total price calculation, and booking validation.
- `CompositeValidator` and booking rules validate booking dates, duplicate booking IDs, overlapping bookings, and customer date conflicts.
- `GlobalExceptionHandler` returns `ProblemDetails` responses with a `traceId` field for easier debugging.
- `DbSeeder` loads initial hotel, room, bed, and price data from `Database/SeedingData.json`.

## Finished

- ASP.NET Core Web API project setup.
- SQLite database connection using Entity Framework Core.
- EF Core migrations configured and applied on app startup.
- Database seeding from JSON.
- Hotel, room, bed, price, customer, and booking models.
- Hotel and room relationships through EF Core.
- Room model includes:
  - Room type
  - Room size
  - Beds
  - Price
  - Hotel relationship
- Booking model includes:
  - Check-in and check-out dates
  - Booking status
  - Number of guests
  - Total price
  - Customer, hotel, and room references
- Customer model includes:
  - First name
  - Last name
  - Email
  - Phone number
  - Personal number
  - Related bookings
- Hotel details endpoint.
- Room list endpoint.
- Room details by ID endpoint.
- Room availability filtering using check-in and check-out dates.
- Optional guest-count filtering for available rooms.
- Date string parsing from query parameters into `DateOnly`.
- Date validation for invalid format and invalid check-out date.
- JSON configuration to avoid reference cycles.
- Swagger UI enabled in development.
- Customer ID lookup and customer creation flow.
- Booking creation through room-specific booking endpoint.
- Booking confirmation response with booking ID, dates, room type, total price, guest count, and status.
- Booking lookup by ID.
- Booking list endpoint for all bookings.
- Booking update endpoint.
- Booking delete endpoint.
- Booking service method for customer booking history.
- Booking validation for invalid date ranges and overlapping bookings.
- Booking exceptions for duplicate booking IDs, missing bookings, invalid dates, invalid customer data, and failed saves.
- Global error response with `ProblemDetails` and `traceId`.
- Unit tests for booking add, update, delete, duplicate ID, and invalid date behavior.

## Available Endpoints

### Hotel

```http
GET /
```

Returns the current hotel branch with rooms, beds, and prices.

Current limitation: the controller uses hard-coded hotel ID `1`.

### Rooms

```http
GET /allRooms
```

Returns all rooms for the current hotel branch.

```http
GET /rooms?checkInDate=2026-06-20&checkOutDate=2026-06-22&numberOfGuests=2
```

Returns rooms available between the given dates. `numberOfGuests` is optional.

Dates must use this format:

```text
yyyy-MM-dd
```

Example:

```text
2026-06-20
```

```http
GET /rooms/{id}?checkInDate=2026-06-20&checkOutDate=2026-06-22
```

Returns one room by ID and calculates total price for the selected date range.

### Customers

```http
POST /api/customer/id
```

Looks up a customer by request data. If no customer is found, the controller creates the customer and returns the new ID.

Example request:

```json
{
  "firstName": "Tony",
  "lastName": "Example",
  "email": "tony@example.com",
  "phoneNumber": "123456789"
}
```

Example response:

```json
{
  "message": "Customer is found in db",
  "id": "00000000-0000-0000-0000-000000000000"
}
```

### Bookings

```http
GET /api/bookings/{bookingId}
```

Returns one booking by ID.

```http
GET /api/bookings
```

Returns all bookings in the system.

```http
POST /api/rooms/{roomId}/bookings
```

Creates a booking for a specific room.

Example request:

```json
{
  "roomId": 1,
  "checkInDate": "2026-06-20",
  "checkOutDate": "2026-06-22",
  "numberOfGuests": 2,
  "customerId": "00000000-0000-0000-0000-000000000000"
}
```

```http
PUT /api/bookings/{bookingReqId}
```

Updates booking date, room, and guest count.

Current issue: the route ID is accepted, but the controller still needs to assign that ID to the booking object before calling `BookingService.UpdateBooking`.

```http
DELETE /api/bookings/{bookingReqId}
```

Deletes a booking by ID.

```http
GET /api/bookings/availability
```

Currently not implemented. The action still throws `NotImplementedException`.

## Error Handling

Unhandled exceptions are handled by `GlobalExceptionHandler`.

The handler returns a `ProblemDetails` response with a `traceId`:

```json
{
  "status": 500,
  "title": "Internal Server Error",
  "detail": "Something went wrong. Please try again later.",
  "instance": "/api/bookings/example",
  "traceId": "0HN..."
}
```

The same trace ID is logged by the backend, so an error from the frontend can be matched with a specific backend log entry.

## Partly Finished

- `RoomService` has add, update, and delete methods, but the controller does not expose admin endpoints for these yet.
- `HotelService` has add, update, delete, and get-all methods, but only the single hotel details endpoint is currently exposed.
- `CustomerService` has get, add, update, and delete methods, but only customer ID lookup/create is exposed by controller.
- `BookingService.GetCustomerBookings` exists, but there is no controller endpoint for customer booking history yet.
- Booking cancellation currently deletes a booking. It does not yet update status to `Cancelled`.
- Booking status enum exists with `Confirmed`, `Cancelled`, and `Pending`, but there is no endpoint dedicated to status changes.
- Payment classes exist as stubs: `Cash`, `Credit`, and `IPayment`.
- `Amenity` model exists but has no fields or behavior.

## Not Finished Yet

- Authentication and authorization.
- Login.
- Signup.
- Logout/session timeout.
- Password reset.
- Admin authentication/login.
- Roles and permissions.
- Property images and room images.
- Amenities data and endpoints.
- Reviews and ratings.
- Payment handling.
- Admin endpoints for hotel, room, and bed management.
- Admin endpoint for booking status updates.
- Customer booking history endpoint.
- Cancellation policy logic.
- Booking cancellation confirmation using `Cancelled` status instead of hard delete.
- Property filtering by price and type.
- Sorting by price and reviews.
- Destination search by city or country.
- Reporting and monitoring.
- Revenue and occupancy tracking.
- Integration tests.
- Cleaner error handling with proper status codes such as `404 Not Found` instead of returning `500` for missing data.
- Remove hard-coded hotel ID `1` and support multiple hotel branches dynamically.
- Decide whether `/rooms` should return all rooms when dates are missing or keep `/allRooms` as the separate endpoint.

## Important Current Issues

### Booking update route ID

`BookingController.UpdateBooking` receives this route parameter:

```csharp
[FromRoute] Guid bookingReqId
```

but the created `Booking` object does not set:

```csharp
Id = bookingReqId
```

Because of that, the service may not update the intended booking.

### Booking availability endpoint

`BookingController.CheckRoomAvailability` is still not implemented:

```csharp
throw new NotImplementedException("Not Implemented yet");
```

Room availability itself is implemented in `RoomService` and exposed through:

```http
GET /rooms?checkInDate=...&checkOutDate=...
```

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

## Testing

Run tests from the backend folder:

```powershell
dotnet test .\BookingSystem.sln
```

Current known issue: the full solution build/test can fail if test code creates `BookingService` without the latest required constructor dependencies.

## What I Learned

- How to create an ASP.NET Core Web API with controllers and services.
- How Entity Framework Core maps C# models to database tables.
- How to use SQLite as a local development database.
- How to use LINQ to query database tables.
- How dependency injection connects controllers to services.
- How to seed database data from a JSON file.
- How to include related data with `Include` and `ThenInclude`.
- How to avoid JSON reference cycle problems with `ReferenceHandler.IgnoreCycles`.
- How to handle exceptions with `IExceptionHandler`.
- How to return traceable API errors with `ProblemDetails`.
- How to use middleware and why middleware order matters.

## Next Steps

1. Fix `BookingController.UpdateBooking` so it passes the route booking ID into the service.
2. Implement `GET /api/bookings/availability` or remove it if `/rooms` remains the availability endpoint.
3. Add customer booking history endpoint.
4. Add cancellation flow that sets booking status to `Cancelled`.
5. Add admin endpoints for hotel and room management.
6. Replace hard-coded hotel ID `1`.
7. Add authentication, roles, and permissions.
8. Add payment, reviews, amenities, and image management.
9. Improve controller status codes and error responses.
10. Update tests to match the latest service constructors.
