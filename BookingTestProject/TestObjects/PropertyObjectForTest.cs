using System.Collections;
using BookingProject;

namespace BookingProject;

public class PropertyObjectForTest:IEnumerable<object[]>
{

    

    public IEnumerator<object[]> GetEnumerator()
    {
        Hotel property = new Hotel("Danish Hotel",DateTime.Now);
        property.Rooms = new List<Room>();
        
        
        Room room1;
        
        Bed bed=new Bed(BedType.Single,true,1);
        List<Bed> beds = new List<Bed>();
        beds.Add(bed);
        Price price = new Price(500);
        room1=new Room(11,RoomType.SingleRoom,beds,price);

        Room room2;
        Bed bed2=new Bed(BedType.Double,true,1);
        List<Bed> beds2 = new List<Bed>();
        beds.Add(bed2);
        room2=new (14,RoomType.DoubleRoom,beds2,price);
        
        property.Rooms.Add(room1);
        property.Rooms.Add(room2);
        property.Id=11;
        
        yield return new object[] {property,room1,room2};
 
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}