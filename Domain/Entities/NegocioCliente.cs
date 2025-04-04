using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NegocioCliente
    {
        public long NegocioId { get; set; }

        [JsonIgnore]
        public Negocio Negocio { get; set; }

        public long ClienteId { get; set; }

        [JsonIgnore]
        public Cliente Cliente { get; set; }
    }
}
