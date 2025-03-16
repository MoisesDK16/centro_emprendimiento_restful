using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class NegocioCliente
    {
        public long NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        public long ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
