using System.Reflection.Metadata.Ecma335;

namespace BookingApp.Models {
    public class Cabin {
        public int Id { get; set;}
        public string Name { get; set;}
        public string ShortDescription { get; set;}
        public string LongDescription { get; set;}
        public int Capacity { get; set;}
        public Decimal PricePerNight { get; set;}

        public List<CabinPhoto> Photos { get; set;} = new List<CabinPhoto>();
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}