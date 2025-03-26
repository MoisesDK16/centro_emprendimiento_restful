using Domain.Enums.Producto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Producto
    {
        public long Id { get; set; }

        //Relaciones
        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public long NegocioId { get; set; }
        public Negocio Negocio { get; set; }

        [AllowNull]
        public long? PromocionId { get; set; }
        public Promocion? Promocion { get; set; }

        [JsonIgnore]
        public List<Stock> Stocks { get; set; } = new List<Stock>();

        //Propiedades
        public string? Codigo { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public Estado Estado { get; set; }
        public decimal Iva { get; set; }
        public string? RutaImagen { get; set; }
    }
}
