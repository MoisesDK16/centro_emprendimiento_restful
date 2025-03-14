using Application.DTOs.Productos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.ProductoC.Queries
{
    public class SelectProductos : IRequest<Response<IEnumerable<ProductoSelectDTO>>>
    {
        public long NegocioId { get; set; }

        public class SelectProductosHandler : IRequestHandler<SelectProductos, Response<IEnumerable<ProductoSelectDTO>>>
        {
            private readonly IReadOnlyRepositoryAsync<Producto> _repository;

            public SelectProductosHandler(IReadOnlyRepositoryAsync<Producto> repository)
            {
                _repository = repository;
            }

            public async Task<Response<IEnumerable<ProductoSelectDTO>>> Handle(SelectProductos request, CancellationToken cancellationToken)
            {
                var products = await _repository.ListAsync(new ProductoSpecification(request.NegocioId));

                var productoSelectDTOs = products
                    .Select(p => new ProductoSelectDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre
                    })
                    .ToList();

                return new Response<IEnumerable<ProductoSelectDTO>>(productoSelectDTOs);
            }

        }
    }
}
