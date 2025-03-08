using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.Categoria.Queries
{
    public class CategoriaById : IRequest<Response<Domain.Entities.Categoria>>
    {
        public long Id { get; set; }
        public class CategoriaByIdHandler : IRequestHandler<CategoriaById, Response<Domain.Entities.Categoria>>
        {
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repository;
            public CategoriaByIdHandler(IRepositoryAsync<Domain.Entities.Categoria> repository)
            {
                _repository = repository;
            }

            public async Task<Response<Domain.Entities.Categoria>> Handle(CategoriaById request, CancellationToken cancellationToken)
            {
                var category = await _repository.GetByIdAsync(request.Id);
                return new Response<Domain.Entities.Categoria>(category);
            }
        }
    }
}
