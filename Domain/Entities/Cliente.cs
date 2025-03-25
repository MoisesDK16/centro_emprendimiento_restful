using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cliente
    {
        public long Id { get; set; }
        public string? Identificacion { get; set; }

        public required string Nombres { get; set; }

        public required string PrimerApellido { get; set; }

        public string? SegundoApellido { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Telefono { get; set; }

        public required string Ciudad { get; set; }

        public string? Direccion { get; set; }

        [JsonIgnore]
        public List<NegocioCliente> NegocioClientes { get; set; } = new List<NegocioCliente>();

    }
}
