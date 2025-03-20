using Domain.Enums.Promocion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Promocion
    {
        public long Id { get; set; }
        public TipoPromocion TipoPromocion { get; set; }

        [AllowNull]
        public decimal? Descuento { get; set; }

        [AllowNull]
        public int? CantidadCompra { get; set; }

        [AllowNull]
        public int? CantidadGratis { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Estado Estado { get; set; } = Estado.ACTIVA;

        //Relaciones
        public List<Producto>? Productos { get; set; } = new List<Producto>();

        public List<Detalle>? Detalles { get; set; } = new List<Detalle>();

        [AllowNull]
        public long NegocioId { get; set; }

        [JsonIgnore]
        public Negocio? Negocio { get; set; }

    }
}
