﻿using backend.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Application.Services
{
    public interface IParticipantService
    {
        Task<ActionResult<IEnumerable<ParticipantDTO>>> GetParticipants(int page = 1, int pageSize = 2);
        Task<ActionResult<ParticipantDTO>> GetParticipant(Guid id);
        Task<IActionResult> PutParticipant(Guid id, ParticipantDTO participantDTO);
        Task<CreateParticipantDTO> PostParticipant(CreateParticipantDTO participantDTO);
        Task<IActionResult> DeleteParticipant(Guid id);
    }
}
