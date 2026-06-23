using System.Runtime.InteropServices.JavaScript;
using BookingProject.Database;
using BookingProject.Models;
using BookingProject.Models.Booking;
using BookingProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingProject.Services;

public class RoomService( AppDbContext context)
{
    public readonly record struct DateRange(DateOnly CheckInDate, DateOnly CheckOutDate);
    
    //crud functions
    public List<RoomDto> GetAll(int hotelId)
    {
        var rooms = context.Rooms.Where(room => room.HotelId == hotelId)
            .Include(room => room.Beds)
            .Include(room => room.Price)
            .ToList();
     
        List<RoomDto> roomsDto = new List<RoomDto>();
        foreach (var room in rooms)
        {
            var count = room.Beds.Select(bed => bed.Quantity).ToList();
            var totalBeds = count.Sum();
            
            RoomDto roomDto = new RoomDto()
            {
                Id = room.Id,
                MaxGuestsAmount =totalBeds,
                Size = room.Size,
                Type = room.Type,
                TotalPrice = room.Price.BasePrice,
            };
            roomsDto.Add(roomDto);
        }
        return roomsDto;
    }
    public List<RoomDto>? GetAvailableRooms(int hotelId,DateOnly checkIn,DateOnly checkOut,int? numberOfGuests = 1)
    {
        var query = context.Rooms
            .Where(room => room.HotelId == hotelId)
            .Where(room => !context.Bookings
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
        
    
        if (rooms.Count==0 )
        {
            return null;
        }

        List<RoomDto> roomsDto = new List<RoomDto>();
        
        int countNumberOfNights=checkOut.DayNumber-checkIn.DayNumber;

        foreach (var room in rooms)
        {
            var count = room.Beds.Select(bed => bed.Quantity).ToList();
            var totalBeds = count.Sum();
            
            RoomDto roomDto = new RoomDto()
            {
                Id = room.Id,
                MaxGuestsAmount =totalBeds,
                Size = room.Size,
                Type = room.Type,
                BasePrice = room.Price.BasePrice,
            };
            roomDto.TotalPrice=roomDto.BasePrice;
            if (countNumberOfNights!=0)
            {
                roomDto.TotalPrice *= countNumberOfNights;

            }
            roomsDto.Add(roomDto);
        }
        
     
        return roomsDto;
    }
    
    public RoomDto GetRoomlByIdInHotel( Room room, DateRange dateRange)
    {
        var queryRoom= context.Hotels.Where(h=>h.Id==room.HotelId)
            .SelectMany(hotel => hotel.Rooms)
            .Include(r => r.Beds)
            .Include(r => r.Price)
            .FirstOrDefault(r => r.Id == room.Id) ?? throw new Exception("Room not found");
        
        var amountOfAllowedGuestsForEachBed = queryRoom.Beds.Select(bed => bed.Quantity).ToList();
        var totalBeds = amountOfAllowedGuestsForEachBed.Sum();
        
        RoomDto roomDto = new RoomDto()
        {
            Id = queryRoom.Id,
            MaxGuestsAmount =totalBeds,
            Size = queryRoom.Size,
            Type = queryRoom.Type,
            BasePrice= queryRoom.Price.BasePrice,
        };
        roomDto.TotalPrice=CalculateTotalPriceForARoom(roomDto, dateRange);
        return roomDto;
    }
    public Room GetRoomlByIdInHotelForUpdate( int roomId,int hotelId)
    {
        var room= context.Hotels.Where(h=>h.Id==hotelId)
            .SelectMany(hotel => hotel.Rooms)
            .Include(room => room.Beds)
            .Include(room => room.Price)
            .FirstOrDefault(room => room.Id == roomId) ?? throw new Exception("Room not found");
        
        return room;
    }

    public void AddRoom(Room room,int hotelId)
    {

        room.HotelId=hotelId;
        context.Rooms.Add(room);
        context.SaveChanges();
    }

    public void Update(Room room,int hotelId)
    {
        var existingRoom = GetRoomlByIdInHotelForUpdate( room.Id, hotelId);
        existingRoom.Type = room.Type;
        existingRoom.Beds = room.Beds;
        existingRoom.Price = room.Price;
        existingRoom.Size  =   room.Size;
        context.SaveChanges();

    }

    public void DeleteRoomFromHotel(int roomId,int hotelId)
    {

        var existingRoom = GetRoomlByIdInHotelForUpdate( roomId, hotelId);
        context.Remove(existingRoom);
        context.SaveChanges();
    
    }
    
    //helper functions
    public double CalculateTotalPriceForARoom(RoomDto room, DateRange dateRange)
    { 
        int countNumberOfNights=dateRange.CheckOutDate.DayNumber -dateRange.CheckInDate.DayNumber;
        Double totalPrice=room.BasePrice*countNumberOfNights;
        return totalPrice;
        
    }
    
}


