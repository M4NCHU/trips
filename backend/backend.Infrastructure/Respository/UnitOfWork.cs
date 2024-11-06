using backend.Domain.Authentication;
using backend.Infrastructure.Authentication;
using backend.Infrastructure.Repository;
using backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace backend.Infrastructure.Respository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TripsDbContext _context;

        public IAuthenticationRepository Auth { get; }
        public ICategoryRepository Categories { get; }
        public IDestinationRepository Destinations { get; }
        public IAccommodationRepository Accommodations { get; }
        public IParticipantRepository Participants { get; }
        public ISelectedPlaceRepository SelectedPlaces { get; }
        public ITripRepository Trips { get; }
        public ITripDestinationRepository TripDestinations { get; }
        public ITripParticipantRepository TripParticipants { get; }
        public IVisitPlaceRepository VisitPlaces { get; }

        public UnitOfWork(TripsDbContext context, ILoggerFactory loggerFactory, IUserStore<UserModel> userStore)
        {
            _context = context;

            Auth = new AuthenticationRepository(
                userStore,
                Options.Create(new IdentityOptions()),
                new PasswordHasher<UserModel>(),
                new List<IUserValidator<UserModel>>(),
                new List<IPasswordValidator<UserModel>>(),
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                loggerFactory.CreateLogger<UserManager<UserModel>>(),
                loggerFactory.CreateLogger<AuthenticationRepository>());
            Categories = new CategoryRepository(_context, loggerFactory.CreateLogger<CategoryRepository>());
            Destinations = new DestinationRepository(_context, loggerFactory.CreateLogger<DestinationRepository>());
            Accommodations = new AccommodationRepository(_context, loggerFactory.CreateLogger<AccommodationRepository>());
            Participants = new ParticipantRepository(_context, loggerFactory.CreateLogger<ParticipantRepository>());
            SelectedPlaces = new SelectedPlaceRepository(_context, loggerFactory.CreateLogger<SelectedPlaceRepository>());
            Trips = new TripRepository(_context, loggerFactory.CreateLogger<TripRepository>());
            TripDestinations = new TripDestinationRepository(_context, loggerFactory.CreateLogger<TripDestinationRepository>());
            TripParticipants = new TripParticipantRepository(_context, loggerFactory.CreateLogger<TripParticipantRepository>());
            VisitPlaces = new VisitPlaceRepository(_context, loggerFactory.CreateLogger<VisitPlaceRepository>());
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}