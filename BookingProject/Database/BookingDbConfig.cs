using BookingProject.Models.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingProject.Database;

public class BookingDbConfig:IEntityTypeConfiguration<Booking>
{
    /// <summary>
    /// This class includes the required configuration of database indexes.
    /// indexes help the databse to execute queries faster that are related to those indexes.
    /// 
    /// </summary>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        //composite database index
        //Helps retrieve bookings by usage of composite database index
        builder.HasIndex(booking => new
        {
            booking.RoomId,
            booking.CheckInDate,
            booking.CheckOutDate
        }).HasDatabaseName("IX_Booking_RoomId_CheckInDate_CheckOutDate");
        
        // Helps retrieve a customer's bookings by date
        builder.HasIndex(booking => new
            {
                booking.CustomerId,
                booking.CheckInDate
            })
            .HasDatabaseName(
                "IX_Bookings_CustomerId_CheckInDate");
    }
}