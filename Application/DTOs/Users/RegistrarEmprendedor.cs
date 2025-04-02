using Application.Feautures.NegocioC.Commands;
using Domain.Enums.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class RegistrarEmprendedor
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string Identificacion { get; set; }
        public required string Telefono { get; set; }
        public required string CiudadOrigen { get; set; }

        //Negocio
        public required string NombreNegocio { get; set; }
        public string? Descripcion { get; set; }
        public required string DireccionNegocio { get; set; }
        public required string TelefonoNegocio { get; set; }
        public long CategoriaNegocio { get; set; }
    }
}
