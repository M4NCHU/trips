﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IVisitPlaceService
    {
        Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces();

        Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(Guid id);

        Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(Guid destinationId);


        Task<IActionResult> PutVisitPlace(Guid id, VisitPlaceDTO visitPlace);

        Task<CreateVisitPlaceDTO> PostVisitPlace(CreateVisitPlaceDTO visitPlaceDTO);

        Task<IActionResult> DeleteVisitPlace(Guid id);
    }
}
