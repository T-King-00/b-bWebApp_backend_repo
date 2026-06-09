using BookingProject.Database;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class HotelService(AppDbContext context) 
{
    public List<Hotel> GetAll()
    {
        try
        {
            var hotels=context.Hotels
                .Include(hotel => hotel.Rooms)
                .ThenInclude(room => room.Beds)
                .Include(hotel => hotel.Rooms)
                .ThenInclude(room => room.Price)
                .ToList();
            return hotels;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Hotel GetHotelById(int hotelId)
    {
        return context.Hotels
                   .Include(hotel => hotel.Rooms)
                   .ThenInclude(room => room.Beds)
                   .Include(hotel => hotel.Rooms)
                   .ThenInclude(room => room.Price)
                   .FirstOrDefault(hotel => hotel.Id == hotelId)
               ?? throw new Exception("Hotel not found");
    }

    public void AddHotel(Hotel hotel)
    {

        try
        {
            ArgumentNullException.ThrowIfNull(hotel);
            context.Hotels.Add(hotel);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }

    public void Update(Hotel hotel)
    {
    
        try
        { 
            ArgumentNullException.ThrowIfNull(hotel);
            var existingHotel = context.Hotels
                                  .FirstOrDefault(h => h.Id == hotel.Id)
                              ?? throw new Exception("Hotel not found");

            existingHotel.Name = hotel.Name;
            existingHotel.Description = hotel.Description;
            existingHotel.Country = hotel.Country;
            existingHotel.City = hotel.City;
            existingHotel.CreationDate = hotel.CreationDate;

            context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
     
    }

    public void Delete(int hotelId)
    {
        try
        {
            var hotel = context.Hotels
                            .Include(h => h.Rooms)
                            .FirstOrDefault(h => h.Id == hotelId)
                        ?? throw new Exception("Hotel not found");

            context.Hotels.Remove(hotel);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}