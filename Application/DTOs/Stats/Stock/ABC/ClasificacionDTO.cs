using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.ABC
{
    public class ClasificacionDTO
    {
        public required string ProductoNombre { get; set; }

        public decimal TotalVendido { get; set; }

        public decimal Acumulado { get; set; }

        public decimal PorcentajeAcumulado { get; set; }

        public required string ClasificacionABC { get; set; }

    }
}
