using backend.Domain.DTOs;
using backend.Domain.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IAccommodationService
    {
        Task<ActionResult<PagedResult<AccommodationDTO>>> GetAccomodations(DestinationFilter filter, int page = 1, int pageSize = 2);

        Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id);

        Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodation);

        Task<ActionResult<AccommodationDTO>> PostAccommodation(CreateAccommodationDTO accomodationDTO, [FromForm] GeoLocationDTO geoLocation);

        Task<IActionResult> DeleteAccommodation(Guid id);
    }
}
