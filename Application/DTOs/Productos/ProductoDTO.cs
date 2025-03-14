using Domain.Enums.Producto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Productos
{
    public class ProductoDTO
    {
        public long Id { get; set; }
        public long CategoriaId { get; set; }
        public string? NombreCategoria { get; set; }
        public long? NegocioId { get; set; }
        public string? NombreNegocio { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public Estado Estado { get; set; }
        public decimal Iva { get; set; }
        public string? RutaImagen { get; set; }

    }
}
