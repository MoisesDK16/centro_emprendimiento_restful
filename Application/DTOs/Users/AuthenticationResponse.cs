using System.Text.Json.Serialization;

namespace Application.DTOs.Users
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public IList<string> Roles { get; set; }
        public bool isVerified { get; set; }
        public string JWToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
