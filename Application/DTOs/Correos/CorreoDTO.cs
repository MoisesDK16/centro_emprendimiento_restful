using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Correos
{
    public class CorreoDTO
    {
        public string? Para { get; set; }
        public required string Asunto { get; set; }
        public required string Contenido { get; set; }
    }
}
