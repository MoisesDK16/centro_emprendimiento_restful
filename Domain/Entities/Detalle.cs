using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Detalle
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        public decimal Total;

        //Relaciones
        public long ProductoId { get; set; }
        public Producto Producto { get; set; }

        public long VentaId { get; set; }    
        public Venta Venta { get; set; }

        [AllowNull]
        public long? PromocionId { get; set; }
        [AllowNull]
        public Promocion? Promocion { get; set; }
    }
}
