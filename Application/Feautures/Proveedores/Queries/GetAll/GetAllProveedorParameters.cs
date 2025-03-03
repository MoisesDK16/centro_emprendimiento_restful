using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.Proveedores.Queries.GetAll
{
    public class GetAllProveedorParameters: RequestParameter
    {
        public string? nombre { get; set; }
        public string? telefono { get; set; }
        public string? ruc { get; set; }
    }
}
