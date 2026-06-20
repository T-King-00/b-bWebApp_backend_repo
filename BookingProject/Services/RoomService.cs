using BookingProject.Database;
using BookingProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class RoomService( AppDbContext _context)
{
    public List<Room> GetAll(int hotelId)
    {
        var rooms = _context.Rooms.Where(room => room.HotelId == hotelId)
            .Include(room => room.Beds)
            .Include(room => room.Price)
            .ToList();
     
        return rooms;
    }
    public List<Room> GetAvailableRooms(int hotelId,DateOnly checkIn,DateOnly checkOut,int? numberOfGuests = null)
    {


        var query = _context.Rooms
            .Where(room => room.HotelId == hotelId)
            .Where(room => !_context.Bookings
                .Any(b => b.RoomId == room.Id
                          && b.Status == Booking.BookingStatus.Confirmed
                          && checkIn < b.CheckOutDate
                          && checkOut > b.CheckInDate));
          
        if (numberOfGuests is not null && numberOfGuests > 0)
        {
            query = query
                .Where(room => room.Beds.Any(bed => bed.Quantity >= numberOfGuests));
        }
        
        
        var rooms=query.
            Include(room => room.Beds)
            .Include(room => room.Price)
            .ToList();
     
        return rooms;
    }
    
    

    public Room GetRoomlByIdInHotel( int roomId,int hotelId)
    {
        return _context.Hotels.Where(h=>h.Id==hotelId)
            .SelectMany(hotel => hotel.Rooms)
            .Include(room => room.Beds)
            .Include(room => room.Price)
            .FirstOrDefault(room => room.Id == roomId) ?? throw new Exception("Room not found");
    }

    public void AddRoom(Room room,int hotelId)
    {

        room.HotelId=hotelId;
        _context.Rooms.Add(room);
        _context.SaveChanges();
    }

    public void Update(Room room,int hotelId)
    {
        var existingRoom = GetRoomlByIdInHotel( room.Id, hotelId);
        existingRoom.Type = room.Type;
        existingRoom.Beds = room.Beds;
        existingRoom.Price = room.Price;
        existingRoom.size  =   room.size;
        _context.SaveChanges();

    }

    public void DeleteRoomFromHotel(int hotelId,int roomId)
    {

        var existingRoom = GetRoomlByIdInHotel(  hotelId,roomId);
        _context.Remove(existingRoom);
        _context.SaveChanges();
    
    }
}
