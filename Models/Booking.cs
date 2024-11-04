namespace BookingApp.Models {
public class Booking {
    public int Id { get; set;}

    public int UserId { get; set;}
    public User User { get; set;}

    public int CabinId { get; set;}
    public Cabin Cabin { get; set;}

    public DateTime CheckInDate { get; set;}
    public DateTime CheckOutDate { get; set;}  
    public string Status { get; set;}

    public List<BookingService> BookingServices { get; set; } = new List<BookingService>();
}

}