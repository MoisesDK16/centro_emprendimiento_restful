using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class ProductoSpecification : Specification<Producto>
    {
        public ProductoSpecification(long productoId, long negocioId)
        {
            Query
                .Include(p => p.Categoria)
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == negocioId)
                .Where(p => p.Id == productoId);
        }

        public ProductoSpecification(long NegocioId, long? CategoriaId)
        {
            Query.Include(p => p.Categoria)
                 .Include(p => p.Negocio);

            Query.Where(p => p.NegocioId == NegocioId);

            if (CategoriaId != 0)
                Query.Where(p => p.CategoriaId == CategoriaId);
        }

        public ProductoSpecification (long NegocioId)
        {
            Query
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == NegocioId);
        }

        public ProductoSpecification(long negocioId, long? categoriaId, string? nombreProducto)
        {
            Query
                .Include(p => p.Categoria)
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == negocioId);

            if (categoriaId != 0)
                Query.Where(p => p.CategoriaId == categoriaId);

            if (!string.IsNullOrEmpty(nombreProducto))
                Query.Search(p => p.Nombre, "%"+nombreProducto+"%"); 

        }

        public ProductoSpecification(long productId, bool isProduct)
        {
            Query.Where(p => p.Id == productId);
        }
    }

}
