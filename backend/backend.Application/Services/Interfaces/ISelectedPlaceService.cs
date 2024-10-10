using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface ISelectedPlaceService
    {
        Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces();
        Task<ActionResult<SelectedPlaceDTO>> GetSelectedPlace(Guid id);
        Task<ActionResult<IEnumerable<SelectedPlaceDTO>>> GetSelectedPlaces(Guid destinationId);
        Task<IActionResult> PutSelectedPlace(Guid id, SelectedPlaceDTO selectedPlaceDTO);
        Task<CreateSelectedPlaceDTO> PostSelectedPlace(CreateSelectedPlaceDTO selectedPlaceDTO);
        Task<IActionResult> DeleteSelectedPlace(Guid id);
    }
}
