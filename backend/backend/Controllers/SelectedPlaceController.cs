using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectedPlaceController : ControllerBase
    {
        private readonly ISelectedPlaceService _selectedPlaceService;

        public SelectedPlaceController(ISelectedPlaceService selectedPlaceService)
        {
            _selectedPlaceService = selectedPlaceService;
        }

        

        // GET: api/SelectedPlace
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlace(int page = 1, int pageSize = 2)
        {
            var selectedPlaces = await _selectedPlaceService.GetSelectedPlaces();
            return selectedPlaces;
        }

        // GET: api/SelectedPlace/destination/1
        [HttpGet("destination/{destinationId}")]
        public async Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId)
        {
            try
            {
                var selectedPlaces = await _selectedPlaceService.GetSelectedPlaces(destinationId);
                return selectedPlaces;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/SelectedPlace/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id)
        {
            return await _selectedPlaceService.GetSelectedPlace(id);
        }

        // PUT: api/SelectedPlace/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlace)
        {
            return await _selectedPlaceService.PutSelectedPlace(id, selectedPlace);
        }

        // POST: api/SelectedPlace

        [HttpPost()]
        public async Task<ActionResult<SelectedPlaceDTO>> PostSelectedPlace([FromForm] SelectedPlaceDTO selectedPlace)
        {
            return await _selectedPlaceService.PostSelectedPlace(selectedPlace);
        }

        // DELETE: api/SelectedPlace/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSelectedPlace(Guid id)
        {
            return await _selectedPlaceService.DeleteSelectedPlace(id);
        }
    }
}
