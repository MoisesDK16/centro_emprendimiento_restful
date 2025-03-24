using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.ABC
{
    public class AnalisisPorMesDTO
    {
        public int Mes { get; set; }

        public decimal CostoInvInicial { get; set; }

        public decimal CostoInvFinal { get; set; }

        public decimal CostoInvPromedio { get; set; }

        public decimal TotalVentas { get; set; }
    }
}
