﻿using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IParticipantService
    {
        Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipants(int page = 1, int pageSize = 2);

        Task<ActionResult<ParticipantDTO>> GetParticipant(int id);

        /*Task<List<ParticipantDTO>> GetParticipantsForTrip(int tripId);*/


        Task<IActionResult> PutParticipant(int id, ParticipantDTO participant);

        Task<ActionResult<ParticipantDTO>> PostParticipant([FromForm] ParticipantDTO participant);

        Task<IActionResult> DeleteParticipant(int id);
    }
}
