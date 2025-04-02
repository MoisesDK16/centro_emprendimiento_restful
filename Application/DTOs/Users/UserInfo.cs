using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Identificacion { get; set; }
        public string CiudadOrigen { get; set; }
        public string? Telefono { get; set; }
    }
}
