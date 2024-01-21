using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IDestinationService
    {
        Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2);

        Task<ActionResult<DestinationDTO>> GetDestination(Guid id);

        Task<List<DestinationDTO>> GetDestinationsForTrip(Guid tripId);


        Task<IActionResult> PutDestination(Guid id, DestinationDTO destination);

        Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destination);

        Task<IActionResult> DeleteDestination(Guid id);
    }
}
