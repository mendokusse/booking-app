namespace BookingApp.DTOs
{
    public class BookingCreateDto
    {
        public int UserId { get; set; }
        public int CabinId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
    }
}
