namespace APBD_CW_3.Models.DTOs;

public class Country_TripGetDTO
{
    public TripGetDTO Trip { get; set; }
    public List<CountryGetDTO> Country { get; set; }
}