using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using Microsoft.Extensions.Hosting;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinationsController : ControllerBase
    {
        private readonly TripsDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ImageService _imageService;

        public DestinationsController(TripsDbContext context, IWebHostEnvironment hostEnvironment, ImageService imageService)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
            this._imageService = imageService;
        }

        // GET: api/Destinations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Destination>>> GetDestinations()
        {
          if (_context.Destinations == null)
          {
              return NotFound();
          }
            return await _context.Destinations
                .Select(x=>new Destination() { 
                    Name = x.Name,
                    Description = x.Description,
                    Location = x.Location,
                    PhotoUrl = String.Format("{0}://{1}{2}/Images/Destinations/{3}", Request.Scheme, Request.Host, Request.PathBase, x.PhotoUrl)
                }).ToListAsync();

        }

        // GET: api/Destinations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Destination>> GetDestination(int id)
        {
          if (_context.Destinations == null)
          {
              return NotFound();
          }
            var destination = await _context.Destinations.FindAsync(id);

            if (destination == null)
            {
                return NotFound();
            }

            return destination;
        }

        // PUT: api/Destinations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDestination(int id, Destination destination)
        {
            if (id != destination.Id)
            {
                return BadRequest();
            }

            _context.Entry(destination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DestinationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Destinations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Destination>> PostDestination([FromForm]Destination destination)
        {
          if (_context.Destinations == null)
          {
              return Problem("Entity set 'TripsDbContext.Destinations'  is null.");
          }
            if (destination.ImageFile != null)
            {
                destination.PhotoUrl = await _imageService.SaveImage(destination.ImageFile, "Destinations");
            }
            _context.Destinations.Add(destination);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDestination", new { id = destination.Id }, destination);
        }

        // DELETE: api/Destinations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestination(int id)
        {
            if (_context.Destinations == null)
            {
                return NotFound();
            }
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }

            _context.Destinations.Remove(destination);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DestinationExists(int id)
        {
            return (_context.Destinations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       

    }
}
