using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("negocio_vendedor")]
    public class NegocioVendedores
    {
        public required long NegocioId { get; set; }
        public required string VendedorId { get; set; }  // Identity usa string como ID

        [ForeignKey("NegocioId")]
        public virtual Negocio Negocio { get; set; } = null!;

    }
}
