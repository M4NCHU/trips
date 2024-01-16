using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAccommodationService
    {
        Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2, string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<ActionResult<AccommodationDTO>> GetAccommodation(int id, string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<IActionResult> PutAccommodation(int id, AccommodationDTO accommodation);

        Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodation);

        Task<IActionResult> DeleteAccommodation(int id);
    }
}
