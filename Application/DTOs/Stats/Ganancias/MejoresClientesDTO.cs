using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Ganancias
{
    public class MejoresClientesDTO
    {
        public string? Identificacion { get; set; }

        public required string NombreCliente { get; set; }

        public required string ApellidoCliente { get; set; }

        public required decimal TotalCompras { get; set; }

        public required int CantidadCompras { get; set; }

        public required decimal PromedioCompraMensual { get; set; }

        public required decimal TotalGanancias { get; set; }

    }
}
