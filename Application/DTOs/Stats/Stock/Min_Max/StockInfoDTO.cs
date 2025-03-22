using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.Min_Max
{
    public class StockInfoDTO
    {
        public required string ProductoNombre { get; set; }
        public int Existencias { get; set; }
        public int Venta { get; set; }
        public double MesesInvt { get; set; }
        public double Promedio12 { get; set; }
        public double StockMin { get; set; }
        public double StockMax { get; set; }
        public required string EstatusInvt { get; set; }
        public double PuntoPedido { get; set; }
        public double CantidadAPedir { get; set; }
    }

}
