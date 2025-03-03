using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Usuario")]
    public class Usuario
    {

        [Required]
        [RegularExpression(@"^(\d{10}|\d{13})$", ErrorMessage = "Longitud errónea de número de identidad")]
        public required string identificacion { get; set; }

        [Required]
        public required string nombre;

        [Required]
        public required string apellido;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public required string correo { get; set; }

        public string? telefono;

        [Required]
        public required Rol rol { get; set; }

        [Required]
        public required string ciudad_origen;

        [Column("id_negocio")]
        public long id_negocio { get; set; }

        [ForeignKey("id_negocio")]
        public virtual Negocio? Negocio { get; set; }

        public virtual List<Negocio>? negocios { get; set; }
    }

    public enum Rol
    {
        ADMIN, EMPRENDEDOR, EMPLEADO, USUARIO
    }
}
