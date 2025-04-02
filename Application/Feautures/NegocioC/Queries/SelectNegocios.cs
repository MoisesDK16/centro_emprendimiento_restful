using Application.DTOs.Negocios;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Queries
{
    public class SelectNegocios : IRequest<Response<IEnumerable<SelectNegocioDTO>>>
    {
        public string EmprendedorId { get; set; }

        public class SelectProductosHandler : IRequestHandler<SelectNegocios, Response<IEnumerable<SelectNegocioDTO>>>
        {
            private readonly IReadOnlyRepositoryAsync<Domain.Entities.Negocio> _repository;

            public SelectProductosHandler(IReadOnlyRepositoryAsync<Domain.Entities.Negocio> repository)
            {
                _repository = repository;
            }

            public async Task<Response<IEnumerable<SelectNegocioDTO>>> Handle(SelectNegocios request, CancellationToken cancellationToken)
            {
                var negocios = await _repository.ListAsync(new NegocioSpecification(null,request.EmprendedorId));
                var negocioSelectDTOs = negocios
                    .Select(n => new SelectNegocioDTO
                    {
                        Id = n.Id,
                        Nombre = n.nombre
                    })
                    .ToList();
                return new Response<IEnumerable<SelectNegocioDTO>>(negocioSelectDTOs);
            }
        }
    }
}
