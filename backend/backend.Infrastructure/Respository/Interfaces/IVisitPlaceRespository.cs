using backend.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IVisitPlaceRepository
    {
        Task<IEnumerable<VisitPlaceDTO>> GetVisitPlacesAsync();
        Task<VisitPlaceDTO> GetVisitPlaceByIdAsync(Guid id);
        Task<IEnumerable<VisitPlaceDTO>> GetVisitPlacesByDestinationAsync(Guid destinationId);
        Task<bool> UpdateVisitPlaceAsync(VisitPlaceDTO visitPlaceDTO);
        Task<CreateVisitPlaceDTO> CreateVisitPlaceAsync(CreateVisitPlaceDTO visitPlaceDTO);
        Task<bool> DeleteVisitPlaceAsync(Guid id);
    }
}
