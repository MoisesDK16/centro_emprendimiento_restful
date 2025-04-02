using Application.Exceptions;
using Application.Interfaces;
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
    public class NegocioById : IRequest<Response<Negocio>>
    {
        public long Id { get; set; } 
    }
    
    public class NegocioByIdHandler : IRequestHandler<NegocioById, Response<Negocio>>
    {
        private readonly IReadOnlyRepositoryAsync<Negocio> _repository;
        public NegocioByIdHandler(IReadOnlyRepositoryAsync<Negocio> repository)
        {
            _repository = repository;
        }
        public async Task<Response<Negocio>> Handle(NegocioById request, CancellationToken cancellationToken)
        {
            var negocio = await _repository.GetByIdAsync(request.Id);
            if (negocio == null) throw new ApiException($"Negocio con ID {request.Id} no encontrado.");
            return new Response<Negocio>(negocio);
        }
    }
}
