using Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Domain.Entities
{
    [Table("proveedor")]
    public class Proveedor: AuditableBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id { get; set; }
        public required string nombre { get; set; }
        public required string telefono { get; set; }
        public required string correo { get; set; }
        public required string direccion { get; set; }
        public required string ruc { get; set; }
    }
}
