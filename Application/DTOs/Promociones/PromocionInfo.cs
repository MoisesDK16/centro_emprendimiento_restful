using Application.DTOs.Productos;
using Domain.Enums.Promocion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Promociones
{
    public class PromocionInfo
    {
        public long Id { get; set; }

        public TipoPromocion TipoPromocion { get; set; }

        public decimal? Descuento { get; set; }

        public int? CantidadCompra { get; set; }

        public int? CantidadGratis { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public List<ProductoDTO> Productos { get; set; } = [];
    }
}
