using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.Application.Authentication;
using backend.Domain.Authentication;

namespace backend.Infrastructure.Authentication
{
    public class TripsDbContext : IdentityDbContext<UserModel>
    {
        public TripsDbContext(DbContextOptions<TripsDbContext> options)
            : base(options)
        {
        }

        public DbSet<TripModel> Trip { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<VisitPlace> VisitPlace { get; set; }
        public DbSet<DestinationModel> Destination { get; set; }
        public DbSet<TripDestinationModel> TripDestination { get; set; }
        public DbSet<SelectedPlaceModel> SelectedPlace { get; set; }
        public DbSet<AccommodationModel> Accommodation { get; set; }
        public DbSet<TripParticipantModel> TripParticipant { get; set; }
        public DbSet<ParticipantModel> Participant { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

             
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DestinationModel>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Destinations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VisitPlace>()
                .HasOne(vp => vp.Destination)
                .WithMany(d => d.VisitPlaces)
                .HasForeignKey(vp => vp.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TripDestinationModel>()
                .HasKey(td => td.Id);

            modelBuilder.Entity<TripDestinationModel>()
                .HasOne(td => td.Trip)
                .WithMany(t => t.TripDestinations)
                .HasForeignKey(td => td.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TripDestinationModel>()
                .HasOne(td => td.Destination)
                .WithMany(d => d.TripDestinations)
                .HasForeignKey(td => td.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TripParticipantModel>()
            .HasKey(tp => tp.Id);

            modelBuilder.Entity<TripParticipantModel>()
                .HasOne(tp => tp.Trip)
                .WithMany(t => t.TripParticipants)
                .HasForeignKey(tp => tp.TripId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<TripParticipantModel>()
                .HasOne(tp => tp.Participant)
                .WithMany(p => p.TripParticipants)
                .HasForeignKey(tp => tp.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the one-to-many relationship between UserModel and TripModel
            modelBuilder.Entity<TripModel>()
                .HasOne<UserModel>(t => t.User) // Trip has one User
                .WithMany(u => u.Trips) // User has many Trips
                .HasForeignKey(t => t.CreatedBy) // Foreign key
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete
        }
    }
}
