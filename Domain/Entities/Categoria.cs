using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    [Table("Categoria")]
    public class Categoria
    {
        [Required]
        public required string nombre { get; set; }

        [Required]
        public required string tipo { get; set; }

        [AllowNull]
        public string? descripcion { get; set; }
    }
}
