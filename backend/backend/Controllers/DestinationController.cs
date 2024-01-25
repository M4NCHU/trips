﻿using System.Collections.Generic;
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
    public class DestinationController : ControllerBase
    {
        private readonly IDestinationService _destinationService;

        public DestinationController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        

        // GET: api/Destinations
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinations(int page = 1, int pageSize = 2)
        {
            var destinations = await _destinationService.GetDestinations(page, pageSize);
            return destinations;
        }

        // GET: api/Destinations/1
        [HttpGet("trip/{tripId}")]
        public async Task<ActionResult<IEnumerable<DestinationDTO>>> GetDestinationsForTrip(Guid tripId)
        {
            try
            {
                var destinationsForTrip = await _destinationService.GetDestinationsForTrip(tripId);
                return destinationsForTrip;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // GET: api/Destinations/5
        [HttpGet("{destinationId}")]
        public async Task<ActionResult<DestinationDTO>> GetDestination(Guid destinationId)
        {
            return await _destinationService.GetDestination(destinationId);
        }

        // PUT: api/Destinations/5
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(Guid id, DestinationDTO destination)
        {
            return await _destinationService.PutDestination(id, destination);
        }

        // POST: api/Destinations
        
        [HttpPost()]
        public async Task<ActionResult<DestinationDTO>> PostDestination([FromForm] DestinationDTO destination)
        {
            return await _destinationService.PostDestination(destination);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(Guid id)
        {
            return await _destinationService.DeleteDestination(id);
        }
    }
}