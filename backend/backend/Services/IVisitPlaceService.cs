﻿using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IVisitPlaceService
    {
        Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlaces(string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<ActionResult<VisitPlaceDTO>> GetVisitPlace(int id, string scheme = "https", string host = "example.com", string pathBase = "/basepath");

        Task<ActionResult<IEnumerable<VisitPlaceDTO>>> GetVisitPlacesByDestination(int destinationId, string scheme = "https", string host = "example.com", string pathBase = "/basepath");


        Task<IActionResult> PutVisitPlace(int id, VisitPlaceDTO visitPlace);

        Task<ActionResult<VisitPlaceDTO>> PostVisitPlace([FromForm] VisitPlaceDTO visitPlace);

        Task<IActionResult> DeleteVisitPlace(int id);
    }
}
