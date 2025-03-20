using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stock
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        public Producto Producto { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no debe ser negativa.")]
        public int Cantidad { get; set; }
        public DateOnly FechaElaboracion { get; set; }
        public DateOnly FechaCaducidad { get; set; }
        public DateTime FechaIngreso { get; set; }

        [JsonIgnore]
        public virtual ICollection<Detalle>? Detalles { get; set; }
    }
}
