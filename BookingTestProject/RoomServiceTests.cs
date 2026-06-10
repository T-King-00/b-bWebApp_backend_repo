using BookingProject;
using BookingProject.Services;
using BookingProject.Database;
using BookingProject.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingTestProject;

public class RoomServiceTests
{
    //Test case 1.
    //on adding a hotel with 1 single room and 1 double room.


    [ClassData(typeof(HotelObjectForTest))]
    [Theory]
    public void AddAHotelWithRooms(Hotel hotel, Room room1, Room room2)
    {
        using(var testDatabase = CreateTestDatabase())
        {
            
            testDatabase.TestServices.HotelService.AddHotel(hotel);
            testDatabase.TestServices.RoomService.AddRoom(room1,hotel.Id);
            testDatabase.TestServices.RoomService.AddRoom(room2,hotel.Id);
            testDatabase.SaveChanges();
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
        
        return new TestDatabase(connection, db);
    }

    private sealed class TestDatabase : IDisposable
    {
        private SqliteConnection Connection { get; }
        private AppDbContext Db { get; }
        public TestServices TestServices { get; }

        
        public TestDatabase(SqliteConnection connection, AppDbContext db)
        {
            Connection = connection;
            Db = db;
            TestServices = new TestServices(db);
        }

        public void SaveChanges()
        {
            try
            {
                Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Test db: Couldnt save changes to db");
            }


            
            
        }
        
        public void Dispose()
        {
            Db.Dispose();
            Connection.Dispose();
        }
    }

    private class TestServices(AppDbContext db)
    {
        public RoomService RoomService { get; } = new(db);
        public HotelService HotelService { get; } = new(db);
    }
}