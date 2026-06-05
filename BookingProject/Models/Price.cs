using System.ComponentModel.DataAnnotations.Schema;

namespace BookingProject;

public class Price
{
   
        public int Id { get; set; }
        public double BasePrice { get; set; }
        
        //nav and reference
        [ForeignKey("RoomId")]
        public Room Room{ get; set; } = null!;

        public Price(double basePrice)
        {
            BasePrice = basePrice;
        }


    
}