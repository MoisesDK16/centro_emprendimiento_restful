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
    }

}
