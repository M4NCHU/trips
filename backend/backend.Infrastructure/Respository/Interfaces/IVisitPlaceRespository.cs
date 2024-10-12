using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;

public interface IVisitPlaceRepository : IRepository<VisitPlaceModel>
{
    Task<IEnumerable<VisitPlaceDTO>> GetVisitPlacesByDestinationAsync(Guid destinationId);
}
