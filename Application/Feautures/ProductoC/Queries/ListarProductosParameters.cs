using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ProductoC.Queries
{
    public class ListarProductosParameters : RequestParameter
    {
        public long CategoriaId { get; set; }

        public long NegocioId { get; set; }

        public string? NombreProducto { get; set; }
    }
}
