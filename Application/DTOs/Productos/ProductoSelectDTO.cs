using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class ProductoSelectDTO
    {
        public long Id { get; set; }
        public required string Nombre { get; set; }
    }
}
