﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Infrastructure.Authentication;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using backend.Application.Services;
using Microsoft.AspNetCore.Hosting;
using backend.Domain.Enums;
using backend.Domain.Mappings;

namespace backend.Infrastructure.Services
{
    public class TripDestinationService : ITripDestinationService
    {
        private readonly TripsDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly BaseUrlService _baseUrlService;
        private readonly string _baseUrl;

        public TripDestinationService(TripsDbContext context, ImageService imageService, IWebHostEnvironment hostEnvironment, BaseUrlService baseUrlService)
        {
            _context = context;
            _imageService = imageService;
            _hostEnvironment = hostEnvironment;
            _baseUrlService = baseUrlService;
            _baseUrl = _baseUrlService.GetBaseUrl();
        }

        

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations()
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }


            var tripDestinations = await _context.TripDestination
                .OrderBy(x => x.Id)
                .Select(x => new TripDestinationDTO
                {
                    Id = x.Id,
                    
                })
                .ToListAsync();

            return tripDestinations;
        }


        public async Task<ActionResult<TripDestinationDTO>> GetTripDestination(Guid id)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            var tripDestination = await _context.TripDestination
        .Where(tp => tp.TripId == id)
        .Select(tp => new TripDestinationDTO
        {
            Id = tp.Id,
            TripId = tp.TripId,
            DestinationId = tp.DestinationId,
            Destination = _context.Destination
                .Where(p => p.Id == tp.DestinationId)
                .Select(p => new DestinationDTO
                {
                    Id = p.Id,
                    PhotoUrl = $"{_baseUrl}/Images/Destination/{p.PhotoUrl}",
                })
                .FirstOrDefault()
        })
        .FirstOrDefaultAsync();

            if (tripDestination == null)
            {
                return new NotFoundResult();
            }


            return tripDestination;

           
        }

        public async Task<ActionResult<IEnumerable<TripDestinationDTO>>> GetTripDestinations(Guid tripId)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            // Utworzenie instancji klasy mapującej, jeśli metody nie są statyczne
            // DestinationMapping destinationMapping = new DestinationMapping();

            var tripDestinations = await _context.TripDestination
                .Include(td => td.Destination)
                .ThenInclude(d => d.VisitPlaces)
                .Where(tp => tp.TripId == tripId)
                .ToListAsync();

            if (tripDestinations == null || !tripDestinations.Any())
            {
                return new NotFoundResult();
            }

            var tripDestinationDTOs = tripDestinations.Select(td => DestinationMapping.MapToTripDestinationDTO(td, _baseUrl)).ToList();
            return new OkObjectResult(tripDestinationDTOs);
        }





        public async Task<IActionResult> PutTripDestination(Guid id, TripDestinationDTO tripDestinationDTO)
        {
            if (id != tripDestinationDTO.Id)
            {
                return new BadRequestResult();
            }

            var tripDestination = new TripDestinationModel
            {
                Id = id,
                
            };

            _context.Entry(tripDestination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TripDestinationExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }

        

        public async Task<(bool IsNew, TripDestinationDTO)> PostTripDestinationAsync([FromForm] TripDestinationDTO tripDestinationDTO)
        {
            if (_context.TripDestination == null)
            {
                throw new InvalidOperationException("Entity set 'TripsDbContext.TripDestination' is null.");
            }

            var currentDate = DateTime.UtcNow;
            Guid tripId = tripDestinationDTO.TripId;

            // Logika tworzenia nowego Trip, jeśli jest to konieczne
            if (tripId == Guid.Empty && !string.IsNullOrWhiteSpace(tripDestinationDTO.Title))
            {
                var newTrip = new TripModel
                {
                    Title = tripDestinationDTO.Title,
                    Status = TripStatus.Planning,
                    StartDate = currentDate,
                    EndDate = currentDate,
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    CreatedBy = tripDestinationDTO.CreatedBy,
                    TotalPrice = 0 // Domyślna wartość, dostosuj w razie potrzeby
                };

                _context.Trip.Add(newTrip);
                await _context.SaveChangesAsync();

                tripId = newTrip.Id;
            }
            else if (tripId != Guid.Empty)
            {
                var existingTrip = await _context.Trip.FindAsync(tripId);
                if (existingTrip == null || existingTrip.Status != TripStatus.Planning)
                {
                    throw new InvalidOperationException("Existing trip not found or not in planning status.");
                }
            }
            else
            {
                throw new ArgumentException("TripId is empty and no title provided for new trip creation.");
            }

            var existingTripDestination = await _context.TripDestination
                .Include(td => td.SelectedPlace)
                .FirstOrDefaultAsync(td => td.DestinationId == tripDestinationDTO.DestinationId && td.TripId == tripId);

            if (existingTripDestination != null)
            {
                existingTripDestination.ModifiedAt = currentDate;
    
                _context.SelectedPlace.RemoveRange(existingTripDestination.SelectedPlace);
                await _context.SaveChangesAsync();

                // Dodawanie nowych SelectedPlace tylko jeśli nie istnieją
                foreach (var selectedPlaceDTO in tripDestinationDTO.SelectedPlaces ?? new List<SelectedPlaceDTO>())
                {
                    bool isSelectedPlaceExists = existingTripDestination.SelectedPlace.Any(sp => sp.VisitPlaceId == selectedPlaceDTO.VisitPlaceId);
        
                    if (!isSelectedPlaceExists)
                    {
                        var selectedPlace = new SelectedPlaceModel
                        {
                            VisitPlaceId = selectedPlaceDTO.VisitPlaceId,
                            TripDestinationId = existingTripDestination.Id,
                            CreatedAt = currentDate,
                            ModifiedAt = currentDate,
                        };
                        _context.SelectedPlace.Add(selectedPlace);
                    }
                }
                await _context.SaveChangesAsync();

                // Możesz zaktualizować tripDestinationDTO, jeśli jest to potrzebne
                // Na przykład, możesz tutaj mapować existingTripDestination na tripDestinationDTO

                return (false, tripDestinationDTO); // false, ponieważ aktualizujemy istniejący wpis
            }
            else
            {
                var newTripDestination = new TripDestinationModel
                {
                    TripId = tripId,
                    DestinationId = tripDestinationDTO.DestinationId,
                    CreatedAt = currentDate,
                    ModifiedAt = currentDate,
                    SelectedPlace = new List<SelectedPlaceModel>()
                };

                if (tripDestinationDTO.SelectedPlaces != null)
                {
                    foreach (var selectedPlaceDTO in tripDestinationDTO.SelectedPlaces)
                    {
                        var selectedPlace = new SelectedPlaceModel
                        {
                            VisitPlaceId = selectedPlaceDTO.VisitPlaceId,
                            // TripDestinationId zostanie ustawione po zapisaniu newTripDestination
                            CreatedAt = currentDate,
                            ModifiedAt = currentDate,
                        };
                        newTripDestination.SelectedPlace.Add(selectedPlace);
                    }
                }

                _context.TripDestination.Add(newTripDestination);
                await _context.SaveChangesAsync();

                foreach (var selectedPlace in newTripDestination.SelectedPlace)
                {
                    selectedPlace.TripDestinationId = newTripDestination.Id;
                }

                await _context.SaveChangesAsync();


                // Aktualizacja tripDestinationDTO.Id na nowo utworzone ID
                tripDestinationDTO.Id = newTripDestination.Id;

                return (true, tripDestinationDTO); // true, ponieważ tworzymy nowy wpis
            }
        }


        public async Task<IActionResult> DeleteTripDestination(Guid id)
        {
            if (_context.TripDestination == null)
            {
                return new NotFoundResult();
            }

            var tripDestination = await _context.TripDestination.FindAsync(id);
            if (tripDestination == null)
            {
                return new NotFoundResult();
            }

            _context.TripDestination.Remove(tripDestination);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<int> CountTripDestinations(Guid tripId)
        {
            if (_context.TripDestination == null)
            {
                return 0; // or throw an exception, handle it as needed
            }

            int count = await _context.TripDestination
                .Where(tp => tp.TripId == tripId)
                .CountAsync();

            return count;
        }

        private bool TripDestinationExists(Guid id)
        {
            return (_context.TripDestination?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    
}
