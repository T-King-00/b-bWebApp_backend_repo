using BookingProject;
using BookingProject.Database;
using BookingProject.Exceptions;
using BookingProject.Models;
using BookingProject.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingTestProject;

public class BookingTests
{
    //tests: AddBooking.
    //conditions: if all data is valid and provided
    //expected: Saved Changes in db
    [Fact]
    public void AddBooking_WhenBookingIsValid_SavesBookingToDatabase()
    {   //arrange
        using var testDatabase = CreateTestDatabase();
        var bookingCreated = CreateBooking(id: 10);
        
        //act
        var bookingSavedAndReturned = testDatabase.BookingService.Add(bookingCreated);
     
        Assert.Equal(bookingCreated,bookingSavedAndReturned);

        var bookingInDatabase = testDatabase.Db.Bookings.Single();
        Assert.Equal(10, bookingInDatabase.Id);
       }

    //tests: AddBooking.
    //conditions: if booking id is already in db
    //expected: ThrowsBookingIdDuplicateException
    [Fact]
    public void AddBooking_WhenBookingIdAlreadyExists_ThrowsBookingIdDuplicateException()
    {
        using var testDatabase = CreateTestDatabase();
        testDatabase.BookingService.Add(CreateBooking(id: 20));
        var duplicateBooking = CreateBooking(id: 20);

        var act = ()=> testDatabase.BookingService.Add(duplicateBooking);

        Assert.Throws<BookingIdDuplicateException>(act);
       
    }

    //tests: AddBooking.
    //conditions: if checkingOutData is not after checking date
    //expected: InvalidBookingDateException
    [Fact]
    public void AddBooking_WhenCheckOutDateIsNotAfterCheckInDate_ThrowsInvalidBookingDateException()
    {
        using var testDatabase = CreateTestDatabase();
        var booking = CreateBooking(
            id: 30,
            checkInDate: new DateOnly(2026, 6, 25),
            checkOutDate: new DateOnly(2026, 6, 25));

        var act = () => testDatabase.BookingService.Add(booking);

        Assert.Throws<InvalidBookingDateException>(act);
        Assert.Empty(testDatabase.Db.Bookings);
    }

    //tests: UpdateBooking.
    //conditions: if new booking data is changed (updating number of guests and checkin date)
    //expected: The changes are saved in db. db entry is updated with new data.
    
    [Fact]
    public void UpdateBooking_WhenBookingIsValid_UpdatesBookingInDatabase()
    {
        //arrange
        using var testDatabase = CreateTestDatabase();
        Booking oldBooking=CreateBooking(1,new DateOnly(2026, 6, 16),new DateOnly(2026, 6, 25));
        testDatabase.Db.Bookings.Add(oldBooking);
        testDatabase.Db.SaveChanges();
        //    IMPORTANT: stop tracking oldBooking
        testDatabase.Db.Entry(oldBooking).State = EntityState.Detached;
        
        //update number of guests and checkin date
        Booking bookingToBeUpdated=new Booking() ;
        bookingToBeUpdated.Id = oldBooking.Id;
        bookingToBeUpdated.CheckOutDate = oldBooking.CheckOutDate;
        bookingToBeUpdated.RoomId = oldBooking.RoomId;
        bookingToBeUpdated.TotalPrice = oldBooking.TotalPrice;
        bookingToBeUpdated.CustomerId = oldBooking.CustomerId;
        
        
        bookingToBeUpdated.NumberOfGuests=1;
        bookingToBeUpdated.CheckInDate=new DateOnly(2026, 6, 17);
        
        testDatabase.BookingService.UpdateBooking(bookingToBeUpdated);
        
        var bookEntryAfterUpdating=testDatabase.Db.Bookings.FirstOrDefault(b=>b.Id==oldBooking.Id)!;
        //assert
        
        Assert.Equal(bookEntryAfterUpdating.NumberOfGuests,bookingToBeUpdated.NumberOfGuests);
        Assert.Equal(bookEntryAfterUpdating.CheckInDate,bookingToBeUpdated.CheckInDate);
        
 
    }

    //tests: DeleteBooking.
    //conditions: if old booking data is deleted 
    //expected: The changes are saved in db. db entry is deleted & throws exception if called with invaild id.

    [Fact]
    public void DeleteBooking_WhenBookingExists_DeletesBookingFromDatabase()
    {
        //arrange
        using var testDatabase = CreateTestDatabase();
        Booking booking=CreateBooking(1,new DateOnly(2026, 6, 16),new DateOnly(2026, 6, 25));
        testDatabase.Db.Bookings.Add(booking);
        testDatabase.Db.SaveChanges();
        
        //ACT
        testDatabase.BookingService.Delete(booking.Id);
        //assert
        var assertExp=()=>testDatabase.BookingService.Get(booking.Id);
        Assert.Throws<BookingNotFoundInDbException>(assertExp);

    }
    
    
    private static Booking CreateBooking(
        int id,
        DateOnly? checkInDate = null,
        DateOnly? checkOutDate = null)
    {
        return new Booking
        {
            Id = id,
            CustomerId = 1,
            RoomId = 1,
            CheckInDate = checkInDate ?? new DateOnly(2026, 6, 16),
            CheckOutDate = checkOutDate ?? new DateOnly(2026, 6, 25),
            NumberOfGuests = 2,
            TotalPrice = 1200
        };
    }

    private static TestDatabase CreateTestDatabase()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var db = new AppDbContext(options);
        db.Database.EnsureCreated();

        SeedRequiredBookingData(db);

        return new TestDatabase(connection, db);
    }

    private static void SeedRequiredBookingData(AppDbContext db)
    {
        var hotel = new Hotel
        {
            Id = 1,
            Name = "Test Hotel",
            City = "Karlskrona",
            Country = "Sweden",
            CreationDate = DateTime.UtcNow,
            Rooms = []
        };

        var room = new Room
        {
            Id = 1,
            HotelId = hotel.Id,
            Hotel = hotel,
            size = 20,
            Type = RoomType.DoubleRoom,
            Price = new Price(100)
        };

        hotel.Rooms.Add(room);

        var customer = new Customer
        {
            Id = 1,
            FirstName = "Tony",
            LastName = "Riad",
            Email = "tony@example.com",
            PhoneNumber = "+46000000000"
        };

        db.Hotels.Add(hotel);
        db.Customers.Add(customer);
        db.SaveChanges();
    }

    private sealed class TestDatabase : IDisposable
    {
        private readonly SqliteConnection _connection;

        public TestDatabase(SqliteConnection connection, AppDbContext db)
        {
            _connection = connection;
            Db = db;
            BookingService = new BookingService(db);
        }

        public AppDbContext Db { get; }
        public BookingService BookingService { get; }

        public void Dispose()
        {
            Db.Dispose();
            _connection.Dispose();
        }
    }
}
