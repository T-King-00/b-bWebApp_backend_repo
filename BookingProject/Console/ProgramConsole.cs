using BookingProject;
using BookingProject.Database;
using BookingProject.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public  class Program0
{
    private static PropertyService _propertyService;

    public static void Main1()
    {
        var databaseDirectory = Path.Combine(AppContext.BaseDirectory, "Database");
        Directory.CreateDirectory(databaseDirectory);

        var databasePath = Path.Combine(databaseDirectory, "BookingAppDbContext.db");
        var connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath
        }.ToString();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .Options;
        
        AppDbContext db = new AppDbContext(options);
        db.Database.Migrate();

        _propertyService = new PropertyService(new PropertyRepo(db));

        bool exit = false;
        
         while (!exit)
         {
             Console.Clear();
             PrintWelcomeMessages();
             Console.WriteLine("1. Manage Properties");
             Console.WriteLine("0. Exit");
             Console.Write("\nSelect an option: ");

             string choice = Console.ReadLine();
             switch (choice)
             {
                 case "1":
                     ManagePropertiesMenu();
                     break;
                 case "0":
                     exit = true;
                     break;
                 default:
                     Console.WriteLine("Invalid option, try again.");
                     Thread.Sleep(1000);
                     break;
             }
         }
    }

    public static void PrintWelcomeMessages()
    {
        Console.WriteLine("===========================");
        Console.WriteLine("   Booking System Admin    ");
        Console.WriteLine("===========================");
    }

    private static void ManagePropertiesMenu()
    {
        bool back = false;
        while (!back)
        {
            Console.Clear();
            Console.WriteLine("--- Manage Properties ---");
            Console.WriteLine("1. View All Properties");
            Console.WriteLine("2. View Property Details");
            Console.WriteLine("3. Add New Property");
            Console.WriteLine("4. Delete Property");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ViewAllProperties();
                    break;
                case "2":
                    ViewPropertyDetails();
                    break;
                case "3":
                    AddNewProperty();
                    break;
                case "4":
                    DeleteProperty();
                    break;
                case "0":
                    back = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    Thread.Sleep(1000);
                    break;
            }
        }
    }

    private static void ViewAllProperties()
    {
        Console.Clear();
        Console.WriteLine("--- All Properties ---");
        var properties = _propertyService.GetAllProperties();
        if (properties.Count == 0)
        {
            Console.WriteLine("No properties found.");
        }
        else
        {
            foreach (var prop in properties)
            {
                PrintPropertyBrief(prop);
            }
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void ViewPropertyDetails()
    {
        Console.Clear();
        Console.WriteLine("--- View Property Details ---");
        var properties = _propertyService.GetAllProperties();

        if (properties.Count == 0)
        {
            Console.WriteLine("No properties found.");
            Thread.Sleep(1500);
            return;
        }

        foreach (var p in properties)
        {
            Console.WriteLine($"ID: {p.Id} | Name: {p.Name}");
        }

        Console.Write("\nEnter the ID of the property to view details (or '0' to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (id == 0) return;

            var prop = _propertyService.GetPropertyById(id);
            if (prop == null)
            {
                Console.WriteLine("Property not found.");
            }
            else
            {
                PrintPropertyFullDetails(prop);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void PrintPropertyBrief(BaseProperty prop)
    {
        int roomsCount = 0;
        if (prop is Hotel hotel)
        {
            roomsCount = hotel.Rooms?.Count ?? 0;
        }
        Console.WriteLine($"ID: {prop.Id} | Name: {prop.Name} | Location: {prop.Address} | Type: {prop.GetType().Name} | Rooms: {roomsCount}");
    }

    private static void PrintPropertyFullDetails(BaseProperty prop)
    {
        Console.Clear();
        Console.WriteLine("========================================");
        Console.WriteLine($" PROPERTY DETAILS: {prop.Name}");
        Console.WriteLine("========================================");
        Console.WriteLine($"ID:       {prop.Id}");
        Console.WriteLine($"Address:  {prop.Address}");
        Console.WriteLine($"Type:     {prop.GetType().Name}");
        Console.WriteLine("----------------------------------------");

        if (prop is Hotel hotel)
        {
            Console.WriteLine("ROOMS:");
            if (hotel.Rooms == null || hotel.Rooms.Count == 0)
            {
                Console.WriteLine("  No rooms found.");
            }
            else
            {
                foreach (var room in hotel.Rooms)
                {
                    Console.WriteLine($"  - Room ID: {room.Id} | Type: {room.RoomType} | Size: {room.size} sqm | Price: {room.Price?.BasePrice:C}/day");
                    if (room.Beds == null || room.Beds.Count == 0)
                    {
                        Console.WriteLine("    No beds found.");
                    }
                    else
                    {
                        Console.WriteLine("     Beds:");
                        foreach (var bed in room.Beds)
                        {
                            Console.WriteLine($"        * {bed.Quantity}x {bed.Type} Bed (Available: {(bed.Available ? "Yes" : "No")})");
                        }
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Details for this property type are limited.");
        }
        Console.WriteLine("========================================");
    }

    private static void AddNewProperty()
    {
        Console.Clear();
        Console.WriteLine("--- Add New Property ---");

        Console.WriteLine("Select Property Type:");
        Console.WriteLine("1. Hotel");
        Console.WriteLine("2. Villa");
        Console.WriteLine("3. Apartment");
        Console.Write("Choice: ");
        string typeChoice = Console.ReadLine();

        BaseProperty newProperty;
        DateTime creationDate = DateTime.Now;

        Console.Write("Enter Name: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Error: Name is required.");
            Thread.Sleep(2000);
            return;
        }

        switch (typeChoice)
        {
            case "1":
                newProperty = new Hotel(name, creationDate);
                break;
            case "2":
                newProperty = new Villa(name, creationDate);
                break;
            case "3":
                newProperty = new Apartment(name, creationDate);
                break;
            default:
                Console.WriteLine("Invalid property type.");
                Thread.Sleep(1500);
                return;
        }

        Console.Write("Enter Address: ");
        newProperty.Address = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newProperty.Address))
        {
            Console.WriteLine("Error: Address is required.");
            Thread.Sleep(2000);
            return;
        }

        if (newProperty is Hotel hotel)
        {
            hotel.Rooms = new List<Room>();

            bool addMoreRooms = true;
            while (addMoreRooms)
            {
                Console.Write("\nDo you want to add a room to this hotel? (y/n): ");
                if (Console.ReadLine()?.ToLower() != "y")
                {
                    addMoreRooms = false;
                    continue;
                }

                Room room = new Room();
                Console.Write("Enter Room Size (sqm): ");
                if (int.TryParse(Console.ReadLine(), out int size)) room.size = size;

                Console.Write("Enter Base Price Per Day: ");
                if (double.TryParse(Console.ReadLine(), out double price))
                {
                    Price p = new Price(price);
                    room.Price = p;
                }

                Console.WriteLine("Select Room Type:");
                foreach (var type in Enum.GetValues<RoomType>())
                {
                    Console.WriteLine($"{(int)type}. {type}");
                }
                Console.Write("Choice: ");
                if (int.TryParse(Console.ReadLine(), out int rtIndex) && Enum.IsDefined(typeof(RoomType), rtIndex))
                {
                    room.RoomType = (RoomType)rtIndex;
                }

                room.Beds = new List<Bed>();
                bool addMoreBeds = true;
                while (addMoreBeds)
                {
                    Console.Write("Do you want to add a bed to this room? (y/n): ");
                    if (Console.ReadLine()?.ToLower() != "y")
                    {
                        addMoreBeds = false;
                        continue;
                    }

                    Console.WriteLine("Select Bed Type:");
                    foreach (var bType in Enum.GetValues<BedType>())
                    {
                        Console.WriteLine($"{(int)bType}. {bType}");
                    }
                    Console.Write("Choice: ");
                    BedType selectedBedType = BedType.Single;
                    if (int.TryParse(Console.ReadLine(), out int btIndex) && Enum.IsDefined(typeof(BedType), btIndex))
                    {
                        selectedBedType = (BedType)btIndex;
                    }

                    Console.Write("Enter Quantity: ");
                    int.TryParse(Console.ReadLine(), out int qty);

                    room.Beds.Add(new Bed(selectedBedType, true, qty));
                }

                hotel.Rooms.Add(room);
            }
        }

        _propertyService.AddProperty(newProperty);

        Console.WriteLine("\nProperty added successfully!");
        Thread.Sleep(1500);
    }

    private static void DeleteProperty()
    {
        Console.Clear();
        Console.WriteLine("--- Delete Property ---");
        var properties = _propertyService.GetAllProperties();
        
        if (properties.Count == 0)
        {
            Console.WriteLine("No properties to delete.");
            Thread.Sleep(1500);
            return;
        }

        foreach (var prop in properties)
        {
            Console.WriteLine($"ID: {prop.Id} | Name: {prop.Name}");
        }

        Console.Write("\nEnter the ID of the property to delete (or '0' to cancel): ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (id == 0) return;
            
            _propertyService.DeletePropertyById(id);
            Console.WriteLine("Property deleted successfully.");
            
        }
        else
        {
            Console.WriteLine("Invalid ID.");
        }
        Thread.Sleep(1500);
    }
}
