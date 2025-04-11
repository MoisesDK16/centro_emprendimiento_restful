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
            Query.Search(n => n.EmprendedorId, "%"+emprendedorId+"%");
        }

        public NegocioSpecification(string vendedorId, bool IsVendedor)
        {
            Query
             .Include(n => n.NegocioVendedores)
             .Where(n => n.NegocioVendedores.Any(v => v.VendedorId == vendedorId));
        }

        public NegocioSpecification(long negocioId, string userId)
        {
            Query.Where(n => n.Id == negocioId && n.EmprendedorId == userId);
            
        }

        public NegocioSpecification(long negocioId, string userId, bool incluirVendedor = false)
        {
            if (incluirVendedor)
            {
                Query
                    .Include(n => n.NegocioVendedores)
                    .Where(n => n.Id == negocioId &&
                        n.NegocioVendedores.Any(v => v.VendedorId == userId));
            }
            else
            {
                Query.Where(n => n.Id == negocioId && n.EmprendedorId == userId);
            }
        }

        public NegocioSpecification(long negocioId, string userId, bool incluirVendedor = false, bool incluirAdmin = false)
        {
            if (incluirAdmin)
            {
                Query.Where(n => n.Id == negocioId);
            }
            else if (incluirVendedor)
            {
                Query
                    .Include(n => n.NegocioVendedores)
                    .Where(n =>
                        n.Id == negocioId &&
                        (n.EmprendedorId == userId || n.NegocioVendedores.Any(v => v.VendedorId == userId)));
            }
            else
            {
                Query.Where(n => n.Id == negocioId && n.EmprendedorId == userId);
            }
        }

    }
}