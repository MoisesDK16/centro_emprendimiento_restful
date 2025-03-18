using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Detalles
{
    public class DetalleDTO
    {
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public long ProductoId { get; set; }
        public long StockId { get; set; }
        public long PromocionId { get; set; }
    }
}
