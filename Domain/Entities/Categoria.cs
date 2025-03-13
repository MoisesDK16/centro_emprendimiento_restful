using Domain.Common;
using Domain.Enums.Categoria;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [Table("categoria")]
    public class Categoria : AuditableBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id { get; set; }
        public required string Nombre { get; set; }

        [EnumDataType(typeof(Tipo), ErrorMessage = "El tipo de categoría no es válido.")]
        public required Tipo Tipo { get; set; }

        public string? Descripcion { get; set; }

        [AllowNull]
        public long? NegocioId { get; set; }

        [ForeignKey("NegocioId")]
        public Negocio? Negocio { get; set; }
    }
}
