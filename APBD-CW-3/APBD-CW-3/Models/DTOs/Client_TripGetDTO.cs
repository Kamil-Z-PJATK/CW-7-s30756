namespace APBD_CW_3.Models.DTOs;

public class Client_TripGetDTO
{
    public ClientGetDTO Client { get; set; }
    public TripGetDTO Trip { get; set; }
    public int RegisteredAt { get; set; }
    public int PaymentDate { get; set; }
}