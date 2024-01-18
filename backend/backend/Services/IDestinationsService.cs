using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IDestinationService
    {
        Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2);

        Task<ActionResult<DestinationDTO>> GetDestination(int id);

        Task<List<DestinationDTO>> GetDestinationsForTrip(int tripId);


        Task<IActionResult> PutDestination(int id, DestinationDTO destination);

        Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destination);

        Task<IActionResult> DeleteDestination(int id);
    }
}
