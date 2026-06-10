using System.Text.Json;
using System.Text.Json.Serialization;
using BookingProject;
using BookingProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Database;

public class DbSeeder
{
    public static async Task SeedAsync(
        AppDbContext context,
        ILogger<DbSeeder> logger,
        CancellationToken cancellationToken)
    {
        using var scope = logger.BeginScope("Database-Seeding");
        logger.LogInformation("Database: seeding data started...");
        try
        {
            var json = await File.ReadAllTextAsync("Database/SeedingData.json", cancellationToken);
            var seedData = JsonSerializer.Deserialize<SeedData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            if (seedData is null)
            {
                return;
            }

            if (await context.Hotels.AnyAsync(cancellationToken))
            {
                logger.LogInformation("    Database already contains data. Seeding skipped.");
                return;
            }

            await context.Hotels.AddRangeAsync(seedData.Hotels);
            await context.Rooms.AddRangeAsync(seedData.Rooms);
            await context.Set<Bed>().AddRangeAsync(seedData.Beds);
            await context.Set<Price>().AddRangeAsync(seedData.Prices);
            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("    Database: seeding data completed.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            logger.LogError(e, "  Database-Error: seeding data failed!");

            throw;
        }
        
    }
    public static  void SeedSync(AppDbContext context, ILogger<DbSeeder> logger)
    {
        using var scope = logger.BeginScope("Database-Seeding");
        logger.LogInformation("Database: seeding data started...");
        try
        {
            var json =  File.ReadAllText("Database/SeedingData.json");
            var seedData = JsonSerializer.Deserialize<SeedData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });

            if (seedData is null)
            {
                return;
            }

            if ( context.Hotels.Any())
            {
                logger.LogInformation("    Database already contains data. Seeding skipped.");
                return;
            }

            context.Hotels.AddRange(seedData.Hotels);
             context.Rooms.AddRange(seedData.Rooms);
             context.Set<Bed>().AddRange(seedData.Beds);
             context.Set<Price>().AddRange(seedData.Prices);
             context.SaveChanges();
            logger.LogInformation("    Database: seeding data completed.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            logger.LogError(e, "  Database-Error: seeding data failed!");

            throw;
        }
        
    }

    private sealed class SeedData
    {
        public List<Hotel> Hotels { get; set; } = [];
        public List<Room> Rooms { get; set; } = [];
        public List<Bed> Beds { get; set; } = [];
        public List<Price> Prices { get; set; } = [];
    }
}
