using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class ProveedorSpecification: Specification<Proveedor>
    {

        public ProveedorSpecification(int pageSize, int pageNumber, string nombre, string ruc)
        {
            Query.Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            if (!string.IsNullOrEmpty(nombre)) Query.Search(p => p.nombre, "%"+nombre+"%");
            if (!string.IsNullOrEmpty(ruc)) Query.Search(p => p.ruc, "%" + ruc + "%");

        }
    }
}
