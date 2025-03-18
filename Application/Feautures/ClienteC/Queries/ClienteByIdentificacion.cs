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

namespace Application.Feautures.ClienteC.Queries
{
    public class ClienteByIdentificacion : IRequest<Response<Cliente>>
    {
        public required string Identificacion { get; set; }

        public class ClienteByIdHandler : IRequestHandler<ClienteByIdentificacion, Response<Cliente>>
        {
            private readonly IReadOnlyRepositoryAsync<Cliente> _repository;
            public ClienteByIdHandler(IReadOnlyRepositoryAsync<Cliente> repository)
            {
                _repository = repository;
            }
            public async Task<Response<Cliente>> Handle(ClienteByIdentificacion request, CancellationToken cancellationToken)
            {
                var cliente = await _repository.FirstOrDefaultAsync(new ClienteSpecification(request.Identificacion), cancellationToken);
                if (cliente == null) throw new ApiException($"Cliente con identificación {request.Identificacion} no encontrado.");
                return new Response<Cliente>(cliente);
            }
        }
    }
}
