using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class ParametrosSpecification : Specification<Parametros>
    {
        public ParametrosSpecification(string nombre) { 

            Query.Search(x => x.Nombre, "%"+nombre+"%");
        }
    }
}
