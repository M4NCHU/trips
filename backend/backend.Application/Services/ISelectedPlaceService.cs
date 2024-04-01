using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface ISelectedPlaceService
    {
        Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces();

        Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id);

        /*Task<List<SelectedPlaceDTO>> GetSelectedPlacesForTrip(int tripId);*/


        Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlace);

        Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId);

        Task<CreateSelectedPlaceDTO> PostSelectedPlace(CreateSelectedPlaceDTO selectedPlaceDTO);

        Task<IActionResult> DeleteSelectedPlace(Guid id);
    }
}
