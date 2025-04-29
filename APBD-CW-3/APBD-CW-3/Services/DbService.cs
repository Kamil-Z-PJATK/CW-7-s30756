using APBD_CW_3.Exceptions;
using APBD_CW_3.Models;
using APBD_CW_3.Models.DTOs;
using Microsoft.Data.SqlClient;


namespace APBD_CW_3.Services;

public class DbService(IConfiguration config): IDbService
{
    private readonly string? _connectionString=config.GetConnectionString("Default");
    public async Task<IEnumerable<Country_TripGetDTO>> GetTripsAndCountrys()
    {
        await using var connection = new SqlConnection(_connectionString);
        string sql="Select * From Trip";
        string sql2 = "SELECT ct.Trip_IdTrip, c.IdCountry, c.Name  FROM Country_Trip ct join Country c on ct.Country_IdCountry = c.IdCountry ";
        var command = new SqlCommand(sql, connection);
       
        await connection.OpenAsync();
        var reader = await command.ExecuteReaderAsync();
        var trips = new List<Country_TripGetDTO>();
        while (await reader.ReadAsync())
        {
            Country_TripGetDTO trip = new Country_TripGetDTO();
            trip.Trip = new TripGetDTO
            {
                IdTrip = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2),
                DateFrom=reader.GetDateTime(3),
                DateTo = reader.GetDateTime(4),
                MaxPeople = reader.GetInt32(5)
            };
            trip.Countrys = new List<CountryGetDTO>();
            
            trips.Add(trip);
        }
        
        command.CommandText = sql2;
        reader.Close();
        var reader2 = await command.ExecuteReaderAsync();
        while (await reader2.ReadAsync())
        {
            for (int i = 0; i < trips.Count; i++)
            {
                if (trips[i].Trip.IdTrip == reader2.GetInt32(0))
                {
                    trips[i].Countrys.Add(new CountryGetDTO
                    {
                        IdCountry = reader2.GetInt32(1),
                        Name = reader2.GetString(2),
                    });
                }
            }
        }
        return trips;
    }

    public async Task<IEnumerable<TripGetDTO>> GetTripsForClient(int klientId)
    {
        List<TripGetDTO> trips = new List<TripGetDTO>();
      await using var connection = new SqlConnection(_connectionString);
      string sql = "SELECT IdClient from Client c ";
      await using var command = new SqlCommand(sql, connection);
      await connection.OpenAsync();
      var reader = await command.ExecuteReaderAsync();
      bool czyIstnieje = false;

      while (await reader.ReadAsync())
      {
          if (klientId == reader.GetInt32(0))
          {
              czyIstnieje = true;
          }
      }
      
      
      if (!czyIstnieje)
      {
          throw new NotFoundException("Client not found");
      }
      reader.Close();
      command.Dispose();
      sql = "Select count(*) from Trip t ";
      command.CommandText = sql;
      var reader2 = await command.ExecuteReaderAsync();
      await reader2.ReadAsync();
      if (reader2.GetInt32(0) == 0)
      {
          throw new NotFoundException("No trips found");
      }
      
      reader2.Close();
      command.Dispose();
      reader2.Dispose();
      sql = "SELECT t.IdTrip, t.Name ,t.Description ,t.DateFrom ,t.DateTo ,t.MaxPeople  FROM Client_Trip ct join Trip t on ct.Trip_IdTrip =t.IdTrip where ct.Client_IdClient =@klientId";
      command.CommandText = sql;
      command.Parameters.AddWithValue("@klientId", klientId);
      var reader3 = await command.ExecuteReaderAsync();
      while (await reader3.ReadAsync())
      {
          trips.Add(new TripGetDTO
          {
              IdTrip = reader3.GetInt32(0),
              Name = reader3.GetString(1),
              Description = reader3.GetString(2),
              DateFrom = reader3.GetDateTime(3),
              DateTo = reader3.GetDateTime(4),
              MaxPeople = reader3.GetInt32(5)
          });
      }
      
      return trips;
        

    }

    public async Task<Client> PostClient(ClientCreateDTO client)
    {
        await using var connection = new SqlConnection(_connectionString);
        string sql = "INSERT INTO Client VALUES ((SELECT Count(*)+1 FROM Client),@FirstName, @LastName, @Email, @Telephone, @Pesel)";
       await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@FirstName", client.FirstName);
        command.Parameters.AddWithValue("@LastName", client.LastName);
        command.Parameters.AddWithValue("@Email", client.Email);
        command.Parameters.AddWithValue("@Telephone", client.Telephone);
        command.Parameters.AddWithValue("@Pesel", client.Pesel);
        await connection.OpenAsync();
        int id = Convert.ToInt32(await command.ExecuteScalarAsync());
        
        return new Client
        {
            IdClient = id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            Email = client.Email,
            Telephone = client.Telephone,
            Pesel = client.Pesel,
        };
        
    }

    public async Task RegisterClientToTrip(int klientId, int tripId)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {
            

            
            
            string sql = "SELECT COUNT(*)  from Trip t WHERE t.IdTrip =@tripId";
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@tripID", tripId);
            await connection.OpenAsync();
            int trId = Convert.ToInt32(await command.ExecuteScalarAsync());

            if (trId == 0)
            {
                throw new NotFoundException("Trip not found");
            }
            
           sql = "SELECT count(*) from Client c WHERE c.IdClient =@klientId ";
           command.CommandText = sql;
           command.Parameters.AddWithValue("@klientId", klientId);
           int clId = Convert.ToInt32(await command.ExecuteScalarAsync());
           if (clId == 0)
           {
               throw new NotFoundException("Client not found");
           }
           command.Dispose();
           await using var command3 = new SqlCommand(sql, connection);
           sql="SELECT t.MaxPeople  from Trip t WHERE t.IdTrip =@tripId";
           command3.CommandText = sql;
           command3.Parameters.AddWithValue("@tripId", tripId);
           var maxPople = Convert.ToInt32(await command3.ExecuteScalarAsync());
           
           sql="SELECT COUNT(*) from Client_Trip ct where ct.Trip_IdTrip =@tripId";
           command3.CommandText = sql;
           // command3.Parameters.AddWithValue("@trId", tripId);
           var currPople = Convert.ToInt32(await command3.ExecuteScalarAsync());

           if (currPople+1 > maxPople)
           {
               throw new TripOverfillException("Max people for trip has been reached");
           }
           command3.Dispose();
            
         
            
            sql = "INSERT INTO Client_Trip VALUES (@clientId, @tripId, 13, 15)";
            await using var command2 = new SqlCommand(sql, connection);
            command2.Parameters.AddWithValue("@clientId", klientId);
            command2.Parameters.AddWithValue("@tripId", tripId);
            var numOfRows = await command2.ExecuteNonQueryAsync();

            if (numOfRows == 0)
            {
                throw new NotFoundException("Client or Trip not found");
            }
            
            
            

        }
    }

    public async Task DelieteClientsRegistration(int klientId, int tripId)
    {
        await using (var connection = new SqlConnection(_connectionString))
        {
            string sql = "SELECT COUNT(*) FROM Client_Trip ct WHERE ct.Client_IdClient = @klientID AND ct.Trip_IdTrip = @tripId";
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@klientId", klientId);
            command.Parameters.AddWithValue("@tripId", tripId);
            connection.Open();
            int ifExists = Convert.ToInt32(await command.ExecuteScalarAsync());
            if (ifExists == 0)
            {
                throw new NotFoundException("Registration doens't exist");
            }
            command.Dispose();

            sql = "DELETE FROM Client_Trip  WHERE Client_IdClient =@klientID AND Trip_IdTrip =@tripId";
            await using var command2 = new SqlCommand(sql, connection);
            command2.Parameters.AddWithValue("@klientId", klientId);
            command2.Parameters.AddWithValue("@tripId", tripId);
            var rowsAffected = await command2.ExecuteNonQueryAsync();

            if (rowsAffected ==0)
            {
                throw new NotFoundException("Registration doens't exist");
            }
            
            
        };
        
    }

    // public async Task<IEnumerable<ClientGetDTO>> GetClients()
    // {
    //     await using var connection = new SqlConnection(_connectionString);
    //     string sql="SELECT * FROM Client";
    //     var command = new SqlCommand(sql, connection);
    //     await connection.OpenAsync();
    //     await using var reader = await command.ExecuteReaderAsync();
    //     
    //     var result = new List<ClientGetDTO>();
    //     while (await reader.ReadAsync())
    //     {
    //         result.Add(new ClientGetDTO()
    //         {
    //             IdClient = reader.GetInt32(0),
    //             FirstName = reader.GetString(1),
    //             LastName = reader.GetString(2),
    //             Email = reader.GetString(3),
    //             Telephone = reader.GetString(4),
    //             Pesel = reader.GetString(5), 
    //         });
    //     }
    //     
    //   return result;
    // }
    
    
}