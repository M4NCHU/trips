using backend.Domain.Authentication;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTOs
{
    public class TripListDTO
    {
       public Guid Id { get; set; }
       public string Title { get; set; }

      public UserDTO User { get; set; }
    }

}
