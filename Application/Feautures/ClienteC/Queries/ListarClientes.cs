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
    public class ListarClientes : IRequest<Response<IEnumerable<Cliente>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? PrimerApellido { get; set; }
        public string? Ciudad { get; set; }

        public class ListarClientesHandler : IRequestHandler<ListarClientes, Response<IEnumerable<Cliente>>>
        {
            private readonly IReadOnlyRepositoryAsync<Cliente> _repository;

            public ListarClientesHandler(IReadOnlyRepositoryAsync<Cliente> repository)
            {
                _repository = repository;
            }

            public async Task<Response<IEnumerable<Cliente>>> Handle(ListarClientes request, CancellationToken cancellationToken)
            {
                var clientes = await _repository.ListAsync(new ClienteSpecification(
                    request.PageNumber,
                    request.PageSize,
                    request.Identificacion,
                    request.Nombres,
                    request.PrimerApellido,
                    request.Ciudad
                ), cancellationToken);

                if (!clientes.Any()) return new Response<IEnumerable<Cliente>>("No se encontraron clientes.");

                return new Response<IEnumerable<Cliente>>(clientes);
            }
        }
    }
}
