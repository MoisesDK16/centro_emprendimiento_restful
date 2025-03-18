
using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Identificacion { get; set; }
        public required string CiudadOrigen { get; set; }
        public string? Telefono { get; set; }
    }
}
