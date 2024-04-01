using backend.Domain.DTOs;
using backend.Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{ 
    public interface IDestinationService
    {
        Task<ActionResult<IEnumerable<ResponseDestinationDTO>>> GetDestinations([FromQuery] DestinationFilter filter, int page = 1, int pageSize = 2);

        Task<ActionResult<DestinationDTO>> GetDestination(Guid id);

        Task<List<DestinationDTO>> GetDestinationsForTrip(Guid tripId);

        Task<IEnumerable<DestinationDTO>> SearchDestinations(string searchTerm);


        Task<IActionResult> PutDestination(Guid id, DestinationDTO destination);

        Task<CreateDestinationDTO> PostDestination(CreateDestinationDTO destinationDTO);

        Task<IActionResult> DeleteDestination(Guid id);
    }
}
