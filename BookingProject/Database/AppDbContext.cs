using Microsoft.EntityFrameworkCore;

using Microsoft.Data.Sqlite;

namespace BookingProject.Database;

//sqllite db
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Booking.Booking> Bookings { get; set; }

    //seeding data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Hotel>().HasData(
            new 
            {
                Id = 1, Name = "Danish Bed and Breakfast",
                City = "Skagen", Country = "Denmark",
                Description = "Danish hotel owned by Pernilles Bed and Breakfast",
                CreationDate = new DateTime(2026,6,8)
            });
        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                HotelId = 1,
                Id = 1,
                RoomType = RoomType.SingleRoom,
                size = 10,

            });
        modelBuilder.Entity<Bed>().HasData(
            new
            {
                Id = 1,
                RoomId = 1,
                Type = BedType.Single,
                Available = true,
                Quantity = 1
            });
        modelBuilder.Entity<Price>().HasData(
            new
            {
                Id = 1,
                RoomId = 1,
                BasePrice = 100.0
            });
        
    }
}
