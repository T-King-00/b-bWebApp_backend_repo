using System.Collections;
using BookingProject;
using BookingProject.Models;

namespace BookingProject;

public class HotelObjectForTest:IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        Hotel hotel = new Hotel( "Bed and breakfast" , DateTime.Now,"Copenhagen","Denmark");
        hotel.Id = 1;
        hotel.Rooms = new List<Room>();
        
        
        Room room1;
        
        Bed bed=new Bed(BedType.Single,1);
        List<Bed> beds = new List<Bed>();
        beds.Add(bed);
        Price price = new Price(500);
        room1=new Room(RoomType.SingleRoom,11,beds, price);
        room1.Id = 1;

        Room room2;
        Bed bed2=new Bed(BedType.Double,1);
        List<Bed> beds2 = new List<Bed>();
        beds.Add(bed2);
        room2=new (RoomType.DoubleRoom,14,beds2, price);
        room2.Id = 2;
        
        
        yield return new object[] {hotel,room1,room2};
 
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}