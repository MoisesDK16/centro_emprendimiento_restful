using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Ganancias
{
    public class GananciasPorCategoriaDTO
    {
        public required string Categoria { get; set; }

        public int CantidadVendida { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal Ventas { get; set; }
        public decimal Ganancias { get; set; }
    }
}
