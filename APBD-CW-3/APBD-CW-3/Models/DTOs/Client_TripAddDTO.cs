namespace APBD_CW_3.Models.DTOs;

public class Client_TripAddDTO
{
    public int IdClient { get; set; }
    public int IdTrip { get; set; }
    public int RegisteredAt { get; set; }
    public int? PaymentDate { get; set; } 
}