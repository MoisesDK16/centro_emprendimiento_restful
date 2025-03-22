using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Ventas
{
    public class VentasMensualesDTO 
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public decimal TotalVentas { get; set; }
    }
}
