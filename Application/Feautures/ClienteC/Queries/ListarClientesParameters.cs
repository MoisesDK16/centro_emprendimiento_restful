using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ClienteC.Queries
{
    public class ListarClientesParameters : RequestParameter
    {
        public string? Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? PrimerApellido { get; set; }
        public string? Ciudad { get; set; }
    }
}
