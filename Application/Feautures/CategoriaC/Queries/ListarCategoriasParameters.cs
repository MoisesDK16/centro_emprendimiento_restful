using Application.Parameters;
using Domain.Enums.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.CategoriaC.Queries
{
    public class ListarCategoriasParameters : RequestParameter
    {
        public required Tipo Tipo { get; set; }

        public long NegocioId { get; set; }
    }
}
