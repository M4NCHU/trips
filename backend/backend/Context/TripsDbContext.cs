using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Authentication
{
    public class TripsDbContext : IdentityDbContext<ApplicationUser>
    {
        public TripsDbContext(DbContextOptions<TripsDbContext> options)
            : base(options)
        {
        }

        /*public DbSet<Employee> Employees { get; set; }*/
        public DbSet<TripModel> Trips { get; set; }
        public DbSet<Category> Category { get; set; }
        /*public DbSet<User> Users { get; set; }
        public DbSet<GuardianParticipant> GuardianParticipants { get; set; }
        public DbSet<TripGuardian> TripGuardians { get; set; }
        public DbSet<TripParticipant> TripParticipants { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<TripEmployee> TripEmployees { get; set; }
        public DbSet<Fee> Fees { get; set; }*/
        public DbSet<VisitPlace> VisitPlaces { get; set; }

        public DbSet<Destination> Destinations { get; set; }
        /*public DbSet<Rating> Ratings { get; set; }
        public DbSet<Guardian> Guardians { get; set; }*/
        public DbSet<TripDestination> TripDestinations { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Destination>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Destinations)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VisitPlace>()
                .HasOne(vp => vp.Destination)
                .WithMany(d => d.VisitPlaces)
                .HasForeignKey(vp => vp.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
