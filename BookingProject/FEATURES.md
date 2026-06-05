# Booking System Features

## Implemented / Partly Implemented Features

- Admin console menu structure for managing properties.
- View all properties.
- View property details by property ID.
- Add a new property.
- Delete a property by ID.
- Store property data in a SQLite database using Entity Framework Core.
- Support multiple property types:
  - Hotel
  - Villa
  - Apartment
- Add hotel rooms.
- Store room details:
  - Room size
  - Room type: `SingleRoom`, `DoubleRoom`, `SuiteRoom`, `FamilyRoom`
  - Base price per day
- Add beds to hotel rooms.
- Store bed details:
  - Bed type: `Single`, `Double`, `King`, `Queen`, `SofaBed`, `BabyCrib`
  - Availability
  - Quantity
- Repository and service layer for property management.
- Basic models for users, bookings, payments, prices, amenities, rooms, beds, and properties.
- BDD scenarios describing:
  - Admin login
  - Admin adding a property listing

## Planned / Stubbed But Not Fully Implemented

- Admin login is described in BDD/tests, but no working login logic exists.
- Booking exists only as an empty model with an ID.
- Payment types exist as empty classes/interfaces: `Cash`, `Credit`, `IPayment`.
- Amenities model exists but has no fields or behavior.
- Updating a property exists in the service interface but throws `NotImplementedException` in the repository.
- Frontend React app exists, but the main `App.jsx` currently renders nothing.
- User model only contains an `Id`.

## Summary

The main working feature area is property management, especially adding, **viewing,** and deleting properties with hotel rooms, beds, and prices.
