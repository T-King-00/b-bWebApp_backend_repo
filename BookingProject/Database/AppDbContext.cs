using Microsoft.EntityFrameworkCore;

using Microsoft.Data.Sqlite;

namespace BookingProject.Database;

//sqllite db
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
   
    public DbSet<BaseProperty> BaseProperties { get; set; }
    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Villa> Villas { get; set; }
    
    public DbSet<User> User { get; set; }
}
