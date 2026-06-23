using System.Text.Json;
using BookingProject.Models;
using BookingProject.Models.Booking;
using Microsoft.EntityFrameworkCore;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookingProject.Database;

//sqllite db
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    
    
}

