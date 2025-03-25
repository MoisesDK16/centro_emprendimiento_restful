using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class ClienteSpecification : Specification<Cliente>
    {
        public ClienteSpecification(string? identificacion, string? nombres, string? primerApellido, string? ciudad)
        {
            //Filtros dinámicos
            if (!string.IsNullOrEmpty(identificacion))
                Query.Where(c => c.Identificacion != null && c.Identificacion.Contains(identificacion));

            if (!string.IsNullOrEmpty(nombres))
                Query.Where(c => c.Nombres.Contains(nombres));

            if (!string.IsNullOrEmpty(primerApellido))
                Query.Where(c => c.PrimerApellido.Contains(primerApellido));

            if (!string.IsNullOrEmpty(ciudad))
                Query.Where(c => c.Ciudad.Contains(ciudad));
        }

        public ClienteSpecification(string identificacion)
        {
            Query.Where(c => c.Identificacion == identificacion);
        }
    }
}
