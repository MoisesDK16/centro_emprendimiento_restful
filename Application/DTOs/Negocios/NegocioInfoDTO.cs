using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Negocios
{
    public class NegocioInfoDTO
    {
        public long Id { get; set; }

        public required string Nombre { get; set; }

        public required string Direccion { get; set; }

        public required string Telefono { get; set; }
        public string? Descripcion { get; set; }
    }

    public class EmprendedorInfoDTO
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Telefono { get; set; }
        public required string CiudadOrigen { get; set; }
        public required string Identificacion { get; set; }
    }
}

