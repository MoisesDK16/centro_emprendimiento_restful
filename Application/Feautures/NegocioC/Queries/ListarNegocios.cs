using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Negocio;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.NegocioC.Queries
{
    public class ListarNegocios : IRequest<PagedResponse<IEnumerable<Negocio>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Nombre { get; set; }
        public string? Telefono { get; set; }
        public Tipo? Tipo { get; set; }
        public Estado? Estado { get; set; }
        public string? EmprendedorId { get; set; }
    }

    public class ListarNegociosHandler : IRequestHandler<ListarNegocios, PagedResponse<IEnumerable<Negocio>>>
    {
        private readonly IReadOnlyRepositoryAsync<Negocio> _repository;
        public ListarNegociosHandler(IReadOnlyRepositoryAsync<Negocio> repository)
        {
            _repository = repository;
        }
        public async Task<PagedResponse<IEnumerable<Negocio>>> Handle(ListarNegocios request, CancellationToken cancellationToken)
        {
            var negocios = await _repository.ListAsync(new NegocioSpecification(
                request.Nombre,
                request.EmprendedorId
            ), cancellationToken);


            var totalRecords = negocios.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);
            var pagedNegocios = negocios.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            if (!pagedNegocios.Any()) throw new ApiException($"No se encontraron negocios");

            return new PagedResponse<IEnumerable<Negocio>>(pagedNegocios, request.PageNumber, request.PageSize, totalPages, totalRecords);
        }
    }
}
