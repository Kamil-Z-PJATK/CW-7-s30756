using APBD_CW_3.Models.DTOs;


namespace APBD_CW_3.Services;

public interface IDbService
{
    public Task<IEnumerable<Country_TripGetDTO>> GetTripsAndCountrys();
    public Task<IEnumerable<Client_TripGetDTO>> GetTripsForClient(int klientId);
    public Task<ClientCreateDTO> PostClient(string firstName, string lastName, string phoneNumber, string email, string pesel);
    public Task RegisterClientToTrip(int klientId, int tripId);
    public Task DelieteClientsRegistration(int klientId, int tripId);
    public Task<IEnumerable<ClientGetDTO>> GetClients();
}