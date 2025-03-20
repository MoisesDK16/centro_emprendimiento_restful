using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock
{
    public class StockReport
    {
        public required string NombreProducto { get; set; }
        public int Stock { get; set; }
    }
}
