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

        public required Tipo tipo { get; set; }

        public required Estado estado { get; set; }
        public required string descripcion { get; set; }

        public List<Categoria> categorias { get; set; } = new List<Categoria>();

        public List<Promocion> Promociones { get; set; } = new List<Promocion>();

        /*[ForeignKey("Emprendedor")]
        public required string EmprendedorId { get; set; }  // Identity usa string como ID
        public virtual ApplicationUser Emprendedor { get; set; } = null!;

        // Relación con Vendedores (Muchos a Muchos)
        public virtual ICollection<ApplicationUser> Vendedores { get; set; } = new List<ApplicationUser>();*/

        /*[Column("categoria_id")]
        public required long categoria_id;

        [ForeignKey("categoria_id")]
        public virtual Categoria categoria { get; set; } = null!;*/
    }
}
