namespace backend.Domain.DTO.Authentication
{
    public class TokenResultDTO
    {
        public bool Success { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string Error { get; set; }
    }
}
