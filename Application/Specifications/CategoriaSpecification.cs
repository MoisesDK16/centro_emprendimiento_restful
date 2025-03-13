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
        public CategoriaSpecification(int pageSize, int pageNumber, Tipo tipo, long NegocioId)
        {
            Query.Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            if (!string.IsNullOrEmpty(tipo.ToString())) Query.Search(c => c.Tipo.ToString(), "%"+tipo.ToString()+"%");

            if (NegocioId != 0) Query.Where(c => c.NegocioId == NegocioId);

        }
    }
}
