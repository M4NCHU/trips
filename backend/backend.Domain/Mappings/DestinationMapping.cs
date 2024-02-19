using backend.Domain.Authentication;
using backend.Domain.DTOs;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Domain.Mappings
{
    public class DestinationMapping
    {
        

        public static DestinationDTO MapToDestinationDTO(DestinationModel model, string baseUrl)
        {
            return new DestinationDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Location = model.Location,
                PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? $"{baseUrl}/Images/Destinations/{model.PhotoUrl}" : null,
                Price = model.Price,
                CategoryId = model.CategoryId,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                Category = model.Category != null ? MapToCategoryDTO(model.Category, baseUrl) : null,
                VisitPlaces = model.VisitPlaces?.Select(vp=>MapToVisitPlaceDTO(vp, baseUrl)).ToList(),
            };
        }
        public static CategoryDTO MapToCategoryDTO(CategoryModel model, string baseUrl)
        {
            return new CategoryDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? $"{baseUrl}/Images/Category/{model.PhotoUrl}" : null,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
            };
        }

        public static VisitPlaceDTO MapToVisitPlaceDTO(VisitPlace model, string baseUrl)
        {
            return new VisitPlaceDTO
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                PhotoUrl = !string.IsNullOrEmpty(model.PhotoUrl) ? $"{baseUrl}/Images/VisitPlace/{model.PhotoUrl}" : null,
                Price = model.Price,
                Duration = model.Duration,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                DestinationId = model.DestinationId
            };
        }

        public static TripDestinationDTO MapToTripDestinationDTO(TripDestinationModel model, string baseUrl)
        {
            return new TripDestinationDTO
            {
                Id = model.Id,
                TripId = model.TripId,
                DestinationId = model.DestinationId,
                Destination = model.Destination != null ? MapToDestinationDTO(model.Destination, baseUrl) : null,
                SelectedPlaces = model.SelectedPlace?.Select(sp=>MapToSelectedPlaceDTO(sp, baseUrl)).ToList(),
                DayNumber = model.DayNumber,
            };
        }

        public static SelectedPlaceDTO MapToSelectedPlaceDTO(SelectedPlaceModel model, string baseUrl)
        {
            return new SelectedPlaceDTO
            {
                Id = model.Id,
                TripDestinationId = model.TripDestinationId,
                VisitPlaceId = model.VisitPlaceId,
                VisitPlace = model.VisitPlace != null ? MapToVisitPlaceDTO(model.VisitPlace, baseUrl) : null, 
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
            };
        }


        public static TripDTO MapToTripDTO(TripModel model, string baseUrl)
        {
            return new TripDTO
            {
                Id = model.Id,
                Status = model.Status,
                StartDate = model.StartDate,
                Title = model.Title,
                EndDate = model.EndDate,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                TotalPrice = model.TotalPrice,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                CreatedBy = model.CreatedBy,
                User = model.User != null ? MapToUserDTO(model.User) : null, // Zakładając, że istnieje odpowiednia metoda mapująca dla UserDTO
                TripDestinations = model.TripDestinations?.Select(td => MapToTripDestinationDTO(td, baseUrl)).ToList(),
                TripParticipants = model.TripParticipants?.Select(tp => MapToTripParticipantDTO(tp)).ToList(),
            };
        }

        public static TripParticipantDTO MapToTripParticipantDTO(TripParticipantModel model)
        {
            return new TripParticipantDTO
            {
                Id = model.Id,
                TripId = model.TripId,
                ParticipantId = model.ParticipantId,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
            };
        }

        public static ParticipantDTO MapToParticipantDTO(ParticipantModel model, string baseUrl)
        {
            return new ParticipantDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                EmergencyContact = model.EmergencyContact,
                EmergencyContactPhone = model.EmergencyContactPhone,
                MedicalConditions = model.MedicalConditions,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt,
                PhotoUrl = model.PhotoUrl,
            };
        }


        public static UserDTO MapToUserDTO(UserModel model)
        {
            return new UserDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
            };
        }


    }
}
