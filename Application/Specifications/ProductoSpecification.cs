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
        public ProductoSpecification(int pageSize, int pageNumber, string Categoria, string Negocio)
        {
            Query.Include(p => p.Categoria)
                 .Include(p => p.Negocio)
                 .Skip(pageSize * (pageNumber - 1))
                 .Take(pageSize);

            if (!string.IsNullOrEmpty(Categoria)) Query.Where(p => p.Categoria != null && p.Categoria.Nombre.Contains(Categoria));
            if (!string.IsNullOrEmpty(Negocio)) Query.Where(p => p.Negocio.nombre != null && p.Negocio.nombre.Contains(Negocio));
        }

        public ProductoSpecification (long NegocioId)
        {
            Query
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == NegocioId);
        }

        public ProductoSpecification(long productId, long NegocioId)
        {
            Query
                .Include(p => p.Categoria)
                .Include(p => p.Negocio)
                .Where(p => p.Id == productId && p.NegocioId == NegocioId);
        }

        public ProductoSpecification(long NegocioId, long categoriaId, Boolean byCategory)
        {
            Query
                .Include(p => p.Categoria)
                .Include(p => p.Negocio)
                .Where(p => p.NegocioId == NegocioId && p.CategoriaId == categoriaId);
        }

        public ProductoSpecification(long productId, bool isProduct)
        {
            Query.Where(p => p.Id == productId);
        }
    }

}
