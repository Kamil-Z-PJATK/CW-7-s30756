using APBD_CW_3.Models.DTOs;
using APBD_CW_3.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APBD_CW_3.Controllers;

[ApiController]
[Route("[controller]")]
public class TripsController(IDbService dbService):ControllerBase
{
   [HttpGet("clients")]
   public async Task<IActionResult> GetAllClients()
   {
      return Ok(await dbService.GetClients());
   }

   [HttpGet]
   public async Task<IActionResult> GetAllTrips(int id)
   {
      return Ok(await dbService.GetTripsAndCountrys());
   }

   
}