using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.VentaC.Queries
{
    public class ListarVentasParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public string? IdentificacionCliente { get; set; }
        public DateOnly FechaInicio { get; set; }

        public DateOnly FechaFin { get; set; }  
    }
}
