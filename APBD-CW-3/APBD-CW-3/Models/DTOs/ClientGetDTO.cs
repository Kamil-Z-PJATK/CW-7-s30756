using System.ComponentModel.DataAnnotations;

namespace APBD_CW_3.Models.DTOs;

public class ClientGetDTO
{
    public int IdClient { get; set; }
    [Length(1,120)]
    public string FirstName { get; set; }
    [Length(1,120)]
    public string LastName { get; set; }
    [Length(1,120)]
    public string Email { get; set; }
    [Length(1,120)]
    public string Telephone { get; set; }
    [Length(1,120)]
    public string Pesel { get; set; }
}