using APBD_CW_3.Models.DTOs;

namespace APBD_CW_3.Services;

public class DbService: IDbService
{
    public Task<IEnumerable<Country_TripGetDTO>> GetTripsAndCountrys(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Client_TripGetDTO>> GetTripsForClient(int klientId)
    {
        throw new NotImplementedException();
    }

    public Task<ClientCreateDTO> PostClient(string firstName, string lastName, string phoneNumber, string email, string pesel)
    {
        throw new NotImplementedException();
    }

    public Task RegisterClientToTrip(int klientId, int tripId)
    {
        throw new NotImplementedException();
    }

    public Task DelieteClientsRegistration(int klientId, int tripId)
    {
        throw new NotImplementedException();
    }
}