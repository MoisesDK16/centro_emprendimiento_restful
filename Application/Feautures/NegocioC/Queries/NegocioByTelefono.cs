using Application.Exceptions;
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
    public class NegocioByTelefono : IRequest<Response<Negocio>>
    {
        public required string telefono { get; set; }

        public class  NegocioByTelefonoHandler : IRequestHandler<NegocioByTelefono, Response<Negocio>>
        {
            private readonly IReadOnlyRepositoryAsync<Negocio> _repository;
            public NegocioByTelefonoHandler(IReadOnlyRepositoryAsync<Negocio> repository)
            {
                _repository = repository;
            }
            public async Task<Response<Negocio>> Handle(NegocioByTelefono request, CancellationToken cancellationToken)
            {
                var negocio = await _repository.FirstOrDefaultAsync(new NegocioSpecification(request.telefono), cancellationToken);
                if (negocio == null) throw new ApiException($"Negocio con telefono {request.telefono} no encontrado.");
                return new Response<Negocio>(negocio);
            }
        }
        
    }
}
