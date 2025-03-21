using Application.Interfaces;
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
    public class ClienteById : IRequest<Response<Cliente>>
    {
        public long Id { get; set; }
    }

    public class ClienteByIdHandler : IRequestHandler<ClienteById, Response<Cliente>>
    {
        private readonly IReadOnlyRepositoryAsync<Cliente> _repository;
        public ClienteByIdHandler(IReadOnlyRepositoryAsync<Cliente> repository)
        {
            _repository = repository;
        }
        public async Task<Response<Cliente>> Handle(ClienteById request, CancellationToken cancellationToken)
        {
            var cliente = await _repository.GetByIdAsync(request.Id);
            if (cliente == null) return new Response<Cliente>("Cliente no encontrado.");
            return new Response<Cliente>(cliente);
        }
    }
}
