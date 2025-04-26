using APBD_CW_3.Exceptions;
using APBD_CW_3.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_CW_3.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController(IDbService dbService):ControllerBase
{
    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetClients([FromRoute]int id)
    {
        try
        {
            return Ok(await dbService.GetTripsForClient(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (NoDataException e)
        {
            return NotFound(e.Message);
        }
        
    }
    
}