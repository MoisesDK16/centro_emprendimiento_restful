using Application.DTOs.Negocios;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Queries
{
    public class SelectNegociosAdmin : IRequest<Response<List<SelectNegocioDTO>>>
    {
    }

    public class SelectNegociosAdminHandler : IRequestHandler<SelectNegociosAdmin, Response<List<SelectNegocioDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Negocio> _repository;

        public SelectNegociosAdminHandler(IReadOnlyRepositoryAsync<Domain.Entities.Negocio> repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<SelectNegocioDTO>>> Handle(SelectNegociosAdmin request, CancellationToken cancellationToken)
        {
            var negocios = await _repository.ListAsync();
            var negocioSelectDTOs = negocios
                .Select(n => new SelectNegocioDTO
                {
                    Id = n.Id,
                    Nombre = n.nombre,
                    Estado = n.estado,
                })
                .ToList();
            return new Response<List<SelectNegocioDTO>>(negocioSelectDTOs);
        }
    }
}
