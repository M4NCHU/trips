using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Domain.Filters;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationsController : ControllerBase
    {
        private readonly IAccommodationService _accommodationService;

        public AccommodationsController(IAccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

        // GET: api/Accommodations
        [HttpGet]
        public async Task<ActionResult<PagedResult<AccommodationDTO>>> GetAccommodations([FromQuery] DestinationFilter filter, int page = 1, int pageSize = 2)
        {
            var accommodations = await _accommodationService.GetAccomodations(filter, page, pageSize);
            return accommodations;
        }

        // GET: api/Accommodations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccommodationDTO>> GetAccommodation(Guid id)
        {
            return await _accommodationService.GetAccommodation(id);
        }

        // PUT: api/Accommodations/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccommodation(Guid id, AccommodationDTO accommodation)
        {
            return await _accommodationService.PutAccommodation(id, accommodation);
        }

        // POST: api/Accommodations

        [HttpPost]
        public async Task<ActionResult<AccommodationDTO>> PostAccommodation(CreateAccommodationDTO accomodationDTO, [FromForm] GeoLocationDTO geoLocation)
        {
            return await _accommodationService.PostAccommodation(accomodationDTO, geoLocation);
        }

        // DELETE: api/Accommodations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            return await _accommodationService.DeleteAccommodation(id);
        }
    }
}
