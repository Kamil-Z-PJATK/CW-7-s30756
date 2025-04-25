using System.ComponentModel.DataAnnotations;

namespace APBD_CW_3.Models.DTOs;

public class TripGetDTO
{
    public int IdTrip { get; set; }
    [Length(1,120)]
    public string Name { get; set; }
    [Length(1,220)]
    public int Description { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int MaxPeople { get; set; }
    
}