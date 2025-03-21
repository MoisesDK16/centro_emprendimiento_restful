using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Venta
    {
        public long Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }

        //Relaciones
        public long ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public long NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        public virtual List<Detalle> Detalles { get; set; } = new List<Detalle>();
    }
}
