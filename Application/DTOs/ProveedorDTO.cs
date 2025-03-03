using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProveedorDTO
    {
        public long Id { get; set; }
        public required string nombre { get; set; }
        public required string telefono { get; set; }
        public required string correo { get; set; }
        public required string direccion { get; set; }
        public required string ruc { get; set; }
    }
}
