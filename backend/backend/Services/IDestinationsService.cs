using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IDestinationService
    {
        Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2, string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<ActionResult<DestinationDTO>> GetDestination(int id, string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<IActionResult> PutDestination(int id, DestinationDTO destination);

        Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destination);

        Task<IActionResult> DeleteDestination(int id);
    }
}
