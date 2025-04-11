using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class NegocioVendedorSpecification : Specification<NegocioVendedores>
    {
        public NegocioVendedorSpecification(string vendedorId)
        {
            Query.Include(x => x.Negocio);
            Query.Search(x => x.VendedorId, vendedorId);
        }
    }
}
