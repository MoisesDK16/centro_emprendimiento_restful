using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Detalles
{
    public class DetalleInfo
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public long ProductoId { get; set; }
        public string ProductoNombre { get; set; }

        public long? PromocionId { get; set; }
        public string? PromocionNombre { get; set; }
    }
}
