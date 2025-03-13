using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stock
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public DateOnly FechaElaboracion { get; set; }
        public DateOnly FechaCaducidad { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
