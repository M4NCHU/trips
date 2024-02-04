
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace backend.Domain.DTO.Authentication
{
    public class LoginResultDTO
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public string? Token { get; set; }
        public AccountDTO? User { get; set; }
    }
}
