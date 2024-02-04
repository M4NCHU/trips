using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;



namespace backend.Domain.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }

    }


}
