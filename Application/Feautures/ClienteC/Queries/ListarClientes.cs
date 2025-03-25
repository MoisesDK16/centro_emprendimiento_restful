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
    public class ListarClientes : IRequest<PagedResponse<IEnumerable<Cliente>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? PrimerApellido { get; set; }
        public string? Ciudad { get; set; }
    }

    public class ListarClientesHandler : IRequestHandler<ListarClientes, PagedResponse<IEnumerable<Cliente>>>
    {
        private readonly IReadOnlyRepositoryAsync<Cliente> _repository;

        public ListarClientesHandler(IReadOnlyRepositoryAsync<Cliente> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResponse<IEnumerable<Cliente>>> Handle(ListarClientes request, CancellationToken cancellationToken)
        {
            var clientes = await _repository.ListAsync(new ClienteSpecification(
                request.Identificacion,
                request.Nombres,
                request.PrimerApellido,
                request.Ciudad
            ), cancellationToken);

            var totalRecords = clientes.Count;
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);
            clientes.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            if (!clientes.Any()) throw new ApiException($"No se encontraron clientes");

            return new PagedResponse<IEnumerable<Cliente>>(clientes, request.PageNumber, request.PageSize, totalPages, totalRecords);
        }
    }
}
