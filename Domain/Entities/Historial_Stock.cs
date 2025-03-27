using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Historial_Stock
    {

        public long Id { get; set; }
        public long ProductoId { get; set; }
        public Producto Producto { get; set; }
        public long NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaCorte { get; set; }
        public int Existencias { get; set; }

        public decimal CostoTotal { get; set; }
    }
}
