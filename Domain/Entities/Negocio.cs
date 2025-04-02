using Domain.Enums.Negocio;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("negocio")]
    public class Negocio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public required string nombre { get; set; }

        public required string direccion { get; set; }

        public required string telefono { get; set; }

        public required Estado estado { get; set; }
        public string descripcion { get; set; }

        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<Promocion>? Promociones { get; set; } = new List<Promocion>();

        public ICollection<NegocioCliente>? NegocioClientes { get; set; }

        public required string EmprendedorId { get; set; }  // Identity usa string como ID

        public ICollection<NegocioVendedores>? NegocioVendedores { get; set; } = new List<NegocioVendedores>();

    }
}
