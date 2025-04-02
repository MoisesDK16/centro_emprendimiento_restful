using Ardalis.Specification;
using Domain.Entities;
using Domain.Enums.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class NegocioSpecification : Specification<Negocio>
    {
        public NegocioSpecification(string? nombre, string? emprendedorId)
        {
            // Filtros dinámicos
            if (!string.IsNullOrEmpty(nombre))
                Query.Where(n => n.nombre != null && n.nombre.Contains(nombre));

            if (!string.IsNullOrEmpty(emprendedorId))
                Query.Where(n => n.EmprendedorId != null && n.EmprendedorId == emprendedorId);
        }

        /*public NegocioSpecification(string telefono)
        {
            Query.Where(n => n.telefono != null && n.telefono == telefono);
        }*/

        public NegocioSpecification(string emprendedorId)
        {
            Query.Search(n => n.EmprendedorId, emprendedorId);
        }
    }
}