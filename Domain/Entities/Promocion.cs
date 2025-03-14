using Domain.Enums.Promocion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Promocion
    {
        public long Id { get; set; }
        public TipoPromocion TipoPromocion { get; set; }

        [AllowNull]
        public decimal Descuento { get; set; }

        [AllowNull]
        public int CantidadCompra { get; set; }

        [AllowNull]
        public int CantidadGratis { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        [EnumDataType(typeof(Estado))]
        public Estado Estado { get; set; }

        //Relaciones
        public List<Producto> Productos { get; set; } = new List<Producto>();

    }
}
