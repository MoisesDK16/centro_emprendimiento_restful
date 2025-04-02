using Application.Parameters;
using Domain.Enums.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Queries
{
    public class ListarNegociosParameter : RequestParameter
    {
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public Tipo Tipo { get; set; }
        public Estado Estado { get; set; }
        public string? EmprendedorId { get; set; }
    }
}
