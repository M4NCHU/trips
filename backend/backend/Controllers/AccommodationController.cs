using backend.Application.Services;
using backend.Domain.DTOs;
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
        public async Task<ActionResult<IEnumerable<AccommodationDTO>>> GetAccommodations(int page = 1, int pageSize = 2)
        {
            var accommodations = await _accommodationService.GetAccommodations(page, pageSize);
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
        public async Task<ActionResult<AccommodationDTO>> PostAccommodation([FromForm] AccommodationDTO accommodation)
        {
            return await _accommodationService.PostAccommodation(accommodation);
        }

        // DELETE: api/Accommodations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccommodation(Guid id)
        {
            return await _accommodationService.DeleteAccommodation(id);
        }
    }
}
