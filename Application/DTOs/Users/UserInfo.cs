using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class UserInfo
    {
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Identificacion { get; set; }
        public required string CiudadOrigen { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? Telefono { get; set; }
    }
}
