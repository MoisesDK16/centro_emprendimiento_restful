using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class CategoriaSpecification : Specification<Categoria>
    {
        public CategoriaSpecification(Tipo tipo, long NegocioId)
        {
            if (!string.IsNullOrEmpty(tipo.ToString())) Query.Search(c => c.Tipo.ToString(), "%"+tipo.ToString()+"%");

            if (NegocioId != 0) Query.Where(c => c.NegocioId == NegocioId);

        }
    }
}
