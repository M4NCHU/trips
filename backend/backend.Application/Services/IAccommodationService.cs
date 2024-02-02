using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IAccommodationService
    {
        Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2);

        Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id);

        Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodation);

        Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodation);

        Task<IActionResult> DeleteAccommodation(Guid id);
    }
}
