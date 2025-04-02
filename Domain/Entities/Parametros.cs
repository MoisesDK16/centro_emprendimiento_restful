using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Parametros
    {
        public long Id { get; set; }
        public required string Nombre { get; set; }
        public decimal Valor { get; set; }
    }
}
