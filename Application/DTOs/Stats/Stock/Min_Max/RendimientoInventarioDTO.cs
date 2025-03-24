using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.Min_Max
{
    public class RendimientoInventarioDTO
    {
        public required string ProductoNombre { get; set; }

        public int InvInicial { get; set; }

        public decimal costoInvInicial { get; set; }

        public int InvFinal { get; set; }

        public decimal TotalVentas { get; set; }

        public decimal ROI { get; set; }
    }
}
