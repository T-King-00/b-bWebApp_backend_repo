using System.ComponentModel.DataAnnotations.Schema;

namespace BookingProject.Models;

public class Price
{
   
        public int Id { get; set; }
        public double BasePrice { get; set; }
        
        //nav and reference
        [ForeignKey("RoomId")]
        public int RoomId { get; set; }
        public Room Room{ get; set; } = null!;
 
        public Price()
        {
        }

        public Price(double basePrice)
        {
            BasePrice = basePrice;
        }

        public Price(int id, int roomId, double basePrice)
        {
            this.Id = id;
            this.BasePrice=basePrice;
            this.RoomId=roomId;
        }
}
