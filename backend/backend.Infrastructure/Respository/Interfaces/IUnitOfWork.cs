using System;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IDestinationRepository Destinations { get; }
        IAccommodationRepository Accommodations { get; }
        IParticipantRepository Participants { get; }
        ISelectedPlaceRepository SelectedPlaces { get; }
        ITripRepository Trips { get; }
        ITripDestinationRepository TripDestinations { get; }
        ITripParticipantRepository TripParticipants { get; }
        IVisitPlaceRepository VisitPlaces { get; }

        Task<bool> SaveChangesAsync();
    }
}
