using APBD_CW_3.Models;
using APBD_CW_3.Models.DTOs;


namespace APBD_CW_3.Services;

public interface IDbService
{
    public Task<IEnumerable<Country_TripGetDTO>> GetTripsAndCountrys();
    public Task<IEnumerable<TripGetDTO>> GetTripsForClient(int klientId);
    public Task<Client> PostClient(ClientCreateDTO client);
    public Task RegisterClientToTrip(int klientId, int tripId);
    public Task DelieteClientsRegistration(int klientId, int tripId);
    // public Task<IEnumerable<ClientGetDTO>> GetClients();
}