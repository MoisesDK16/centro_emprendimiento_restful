using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Enums.Categoria;
using MediatR;

namespace Application.Feautures.CategoriaC.Queries
{

    public class ListarCategorias : IRequest<PagedResponse<IEnumerable<Domain.Entities.Categoria>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required Tipo Tipo { get; set; }

        public long NegocioId { get; set; }

        public class ListarCategoriasHandler : IRequestHandler<ListarCategorias, PagedResponse<IEnumerable<Domain.Entities.Categoria>>>
        {
            private readonly IReadOnlyRepositoryAsync<Domain.Entities.Categoria> _repository;
            public ListarCategoriasHandler(IReadOnlyRepositoryAsync<Domain.Entities.Categoria> repository)
            {
                _repository = repository;
            }
            public async Task<PagedResponse<IEnumerable<Domain.Entities.Categoria>>> Handle(ListarCategorias request, CancellationToken cancellationToken)
            {
                var categories = await _repository.ListAsync(
                        new CategoriaSpecification(request.Tipo, request.NegocioId),
                        cancellationToken 
                    ).ConfigureAwait(false);

                var TotalRecords = categories.Count;
                var TotalPages = (int)Math.Ceiling((double)categories.Count / request.PageSize);
                categories.Skip(request.PageNumber - 1 * request.PageSize).Take(request.PageSize);

                return new PagedResponse<IEnumerable<Domain.Entities.Categoria>>(categories, request.PageNumber,
                    request.PageSize, TotalPages, TotalRecords);
            }
        }
    }
}
