using BookingProject;
using BookingProject.Database;
using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models;
using BookingProject.Models.Booking;
using BookingProject.Services;
using BookingProject.Validators;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingTestProject.UnitTests.ServiceTests;

public class BookingServiceTests
{
    private static readonly DateOnly TodayDate = new DateOnly(2026, 6, 23);
    //tests: AddBooking.
    //conditions: if all data is valid and provided
    //expected: Saved Changes in db
    [Fact]
    public void AddBooking_WhenBookingIsValid_SavesBookingToDatabase()
    {   //arrange
        using var testDatabase = CreateTestDatabase();
        var bookingCreated = CreateBooking( new Guid("422B658C-DEB5-446D-9C34-6E6E10E0BD38"));
        
        //act
        var bookingSavedAndReturned = testDatabase.BookingService.Add(bookingCreated);
        
        
        //excepted,actual
        Assert.Equal(bookingCreated.Id,bookingSavedAndReturned.BookingId);

       }

    //tests: AddBooking.
    //conditions: if a new booking is valid, its id is already in db
    //expected: ThrowsBookingIdDuplicateException
    [Fact]
    public void AddBooking_WhenBookingIdAlreadyExists_ThrowsBookingIdDuplicateEx()
    {
        using var testDatabase = CreateTestDatabase();
        var bookingService = testDatabase.BookingService;
        Booking bookingToAdd = CreateBooking();
        bookingService.Add(bookingToAdd);
        
        testDatabase.Db.SaveChanges();
        //add duplicate booking
        var firstBookingId=bookingToAdd.Id;
        var duplicateBooking = CreateBooking(firstBookingId);
        
        Assert.ThrowsAny<AggregateException>(() => bookingService.Add(duplicateBooking));

    }

    //tests: AddBooking.
    //conditions: if checkingOutData is not after checking date
    //expected: InvalidBookingDateException
    [Fact]
    public void AddBooking_WhenCheckOutDateIsNotAfterCheckInDate_ThrowsInvalidBookingDateEx()
    {
        using var testDatabase = CreateTestDatabase();
        var bookingService = testDatabase.BookingService;
        var booking = CreateBooking(
            id :new Guid("422B658C-DEB5-446D-9C34-6E6E10E0B330"),
            checkInDate: new DateOnly(2026, 6, 25),
            checkOutDate: new DateOnly(2026, 6, 25));
            
        var act = () => bookingService.Add(booking);
            
        Assert.Throws<AggregateException>(act);
        Assert.Empty(testDatabase.Db.Bookings);
    }

    //tests: AddBooking.
    //conditions: if booking is not valid due to date bugs
    //expected: InvalidBookingDateException
    [Fact]
    public void AddBooking_WhenBookingIsInvalid_ThrowsInvalidBookingEx()
    {
        using var testDatabase = CreateTestDatabase();
        var booking = CreateBooking(
            new Guid("422B658C-DEB5-446D-9C34-6E6E10E0B330"),
            checkInDate:TodayDate,
            checkOutDate: TodayDate );
        booking.RoomId = 100;
        var bookingService = testDatabase.BookingService;

        var act = () => bookingService.Add(booking);

        Assert.Throws<AggregateException>(act);
        Assert.Empty(testDatabase.Db.Bookings);
    }
    
    
    //tests: UpdateBooking.
    //conditions: if new booking data is changed (updating number of guests and check-in date)
    //expected: The changes are saved in db. db entry is updated with new data.
    
    [Fact]
    public void UpdateBooking_WhenBookingIsValid_UpdatesBookingInDatabase()
    {
        //arrange
        using var testDatabase = CreateTestDatabase();
        Booking oldBooking=CreateBooking(new Guid("522B658C-DEB5-446D-9C34-6E6E10E0B305"),TodayDate,TodayDate.AddDays(5));
        testDatabase.Db.Bookings.Add(oldBooking);
        testDatabase.Db.SaveChanges();
        //    IMPORTANT: stop tracking oldBooking
        testDatabase.Db.Entry(oldBooking).State = EntityState.Detached;
        
        //update number of guests and check-in date
        Booking bookingToBeUpdated = CreateBooking(
            id: oldBooking.Id,
            checkInDate: TodayDate.AddDays(2),
            checkOutDate: TodayDate.AddDays(5));
        
        bookingToBeUpdated.TotalPrice = oldBooking.TotalPrice;
        bookingToBeUpdated.CustomerId = oldBooking.CustomerId;
        
        bookingToBeUpdated.NumberOfGuests=1;
        
        //act 
        testDatabase.BookingService.UpdateBooking(bookingToBeUpdated);
        
        var bookEntryAfterUpdating=testDatabase.Db.Bookings.AsNoTracking().Single(b=>b.Id==oldBooking.Id);
        //assert
        //(Expected,actual)
        Assert.Equal(bookingToBeUpdated.NumberOfGuests,bookEntryAfterUpdating.NumberOfGuests);
        Assert.Equal(bookingToBeUpdated.CheckInDate,bookEntryAfterUpdating.CheckInDate);
        
 
    }

    //tests: UpdateBooking.
    //conditions: if new booking data is changed (updating check-in date,check-out date)
    //expected: throws invalid booking date exception
    [Fact]
    public void UpdateBooking_WhenBookingDateIsInvalid_ThrowsInvalidBookingDateEx()
    {
        //arrange
        using var testDatabase = CreateTestDatabase();
        Booking oldBooking=CreateBooking(new Guid("122B658C-DEB5-446D-9C34-6E6E10E0B305"),TodayDate,TodayDate.AddDays(5));
        testDatabase.Db.Bookings.Add(oldBooking);
        testDatabase.Db.SaveChanges();
        //    IMPORTANT: stop tracking oldBooking
        testDatabase.Db.Entry(oldBooking).State = EntityState.Detached;
        
        //update number of guests and check-in date
        Booking bookingToBeUpdated = CreateBooking(
            id: oldBooking.Id,
            checkInDate: new DateOnly(2026, 6, 20),
            checkOutDate: oldBooking.CheckOutDate);
        var bookingService = testDatabase.BookingService;
        
        //act 
        var act = () => { bookingService.UpdateBooking(bookingToBeUpdated);};

        //assert
        Assert.Throws<AggregateException>(act);
    }
    
    //tests: DeleteBooking functionality from BookingService
    //conditions: booking exists and valid in db
    //expected: The changes are saved in db. db entry is deleted from db
    [Fact]
    public void DeleteBooking_WhenBookingExists_DeletesBookingFromDatabase()
    {
        //arrange
        using var testDatabase = CreateTestDatabase();
        Booking booking=CreateBooking(new Guid("122B658C-DEB5-446D-9C34-6E6E10E0B305"),new DateOnly(2026, 6, 16),new DateOnly(2026, 6, 25));
        testDatabase.Db.Bookings.Add(booking);
        testDatabase.Db.SaveChanges();
        
        //ACT on booking service
        testDatabase.BookingService.Delete(booking.Id);
        //assert
        
        //test db directly instead of testing another functionality of booking service 
        Assert.Empty(testDatabase.Db.Bookings);

    }
    
    //tests: DeleteBooking functionality from BookingService
    //conditions: booking doesn't exist.
    //expected: Throw error
    [Fact]
    public void DeleteBooking_WhenBookingNotExists_ThrowsBookingNotFoundInDbException()
    {
        //arrange
        
        Guid bookingId=new Guid("522B658C-DEB5-446D-0000-6E6E10E0B305");
        using var testDatabase = CreateTestDatabase();
        var bookingService = testDatabase.BookingService;

        //ACT on booking service
        var act=()=>
        {
            bookingService.Delete(bookingId);
        };
        
        //assert
        
        Assert.Throws<CustomExceptions.BookingNotFoundInDbException>(act);

    }

    
    private static Booking CreateBooking(
        Guid? id = null,
        DateOnly? checkInDate = null,
        DateOnly? checkOutDate = null)
    {
        return new Booking
        {
            Id = id ?? Guid.NewGuid(),
            HotelId = 1,
            CustomerId = 1,
            RoomId = 1,
            
            CheckInDate = checkInDate ?? TodayDate,
            CheckOutDate = checkOutDate ?? TodayDate.AddDays(5),
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
            Size = 20,
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
            Logger<BookingService> logger = new(new LoggerFactory());
            
            var rules=new List<IBookingRule>
            {
                new BookingMustNotOverlapRule(db),
                new BookingNoDuplicateIdRule(db),
                new DateValidationRule()
            };
      
            
            CompositeValidator cv = new CompositeValidator(rules);
            RoomService roomService = new(db);
            BookingService = new BookingService(db,logger,cv,roomService);
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
