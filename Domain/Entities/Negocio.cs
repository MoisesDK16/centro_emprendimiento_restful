using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Negocio")]
    public class Negocio
    {
        [StringLength(30)]
        [Column("nombre")]
        public required string nombre { get; set; }

        [StringLength(30)]
        [Column("ruc")]
        public required string ruc { get; set; }

        [StringLength(100)]
        [Column("direccion")]
        public required string direccion { get; set; }

        [StringLength(15)]
        [Column("telefono")]
        public required string telefono { get; set; }

        [StringLength(60)]
        [Column("tipo_negocio")]
        public required string tipoNegocio { get; set; }

        [StringLength(10)]
        [Column("estado")]
        public required string estado { get; set; }

        [StringLength(200)]
        [Column("descripcion")]
        public required string descripcion { get; set; }

        //Foreaneas

        [Column("id_emprendedor")]
        public required long id_emprendedor { get; set; }

        [ForeignKey("id_emprendedor")]
        public virtual Usuario Emprendedor { get; set; } = null!;

        public virtual List<Usuario> Vendedores { get; set; } = new();

        [Column("id_categoria")]
        public required long id_categoria;

        [ForeignKey("id_categoria")]
        public virtual Categoria categoria { get; set; } = null!;
    }
}
