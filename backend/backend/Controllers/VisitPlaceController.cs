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
    public class VisitPlaceController : ControllerBase
    {
        private readonly IVisitPlaceService _visitPlaceService;

        public VisitPlaceController(IVisitPlaceService visitPlaceService)
        {
            _visitPlaceService = visitPlaceService;
        }

        // GET: api/VisitPlaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces()
        {
            var visitPlaces = await _visitPlaceService.GetVisitPlaces();
            return visitPlaces;
        }


        // GET: api/VisitPlaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id)
        {
            return await _visitPlaceService.GetVisitPlace(id);
        }

        // GET: api/VisitPlaces/destination/1
        [HttpGet("destination/{destinationId}")]
        public async Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId)
        {
            return await _visitPlaceService.GetVisitPlacesByDestination(destinationId);
        }


        // PUT: api/VisitPlaces/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO visitPlace)
        {
            return await _visitPlaceService.PutVisitPlace(id, visitPlace);
        }

        // POST: api/VisitPlaces

        [HttpPost()]
        public async Task<ActionResult<VisitPlaceDTO>> PostVisitPlace([FromForm] VisitPlaceDTO visitPlace)
        {
            return await _visitPlaceService.PostVisitPlace(visitPlace);
        }

        // DELETE: api/VisitPlaces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitPlace(Guid id)
        {
            return await _visitPlaceService.DeleteVisitPlace(id);
        }
    }
}