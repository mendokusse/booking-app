namespace BookingApp.Models {
public class CabinPhoto {
    public int Id { get; set;}
    public int CabinId { get; set;}
    public Cabin Cabin { get; set; } 
    public string Url { get; set;}
}

}