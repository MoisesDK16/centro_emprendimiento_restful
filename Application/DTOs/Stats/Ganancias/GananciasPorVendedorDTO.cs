using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Ganancias
{
    public class GananciasPorVendedorDTO
    {
        public required string NombreVendedor { get; set; }

        public required string ApellidoVendedor { get; set; }

        public required decimal Ganancias { get; set; }
    }
}
