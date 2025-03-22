using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Ganancias
{
    public class GananciasPorProductoDTO
    {
        public required string Nombre { get; set; }
        public int CantidadVendida { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal GananciaTotal { get; set; }
    }

}
