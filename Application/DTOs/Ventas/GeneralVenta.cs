using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Ventas
{
    public class GeneralVenta
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public long ClienteId { get; set; }
        public long NegocioId { get; set; }
    }
}
