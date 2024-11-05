namespace BookingApp.DTOs {
    public class CabinDto {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Capacity { get; set; }
        public Decimal PricePerNight { get; set; }
    }
}