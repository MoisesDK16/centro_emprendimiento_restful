using Application.DTOs.Productos;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using MediatR;
namespace Application.Feautures.ProductoC.Queries
{
    public class ListarProductos : IRequest<PagedResponse<IEnumerable<ProductoDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long CategoriaId { get; set; }
        public long NegocioId { get; set; }
        public string? NombreProducto { get; set; }

        public class ListarProductosHandler : IRequestHandler<ListarProductos, PagedResponse<IEnumerable<ProductoDTO>>>
        {
            private readonly IReadOnlyRepositoryAsync<Domain.Entities.Producto> _repository;

            public ListarProductosHandler(IReadOnlyRepositoryAsync<Domain.Entities.Producto> repository)
            {
                _repository = repository;
            }

            public async Task<PagedResponse<IEnumerable<ProductoDTO>>> Handle(ListarProductos filter, CancellationToken cancellationToken)
            {
                var products = await _repository.ListAsync(
                        new ProductoSpecification(filter.NegocioId, filter.CategoriaId, filter.NombreProducto)
                    );

                Console.WriteLine("productos "+ products.Count);

                List<ProductoDTO> productosDTO = new();

                foreach (var product in products)
                {
                    productosDTO.Add(new ProductoDTO
                    {
                        Id = product.Id,
                        CategoriaId = product.CategoriaId,
                        NombreCategoria = product.Categoria.Nombre,
                        NegocioId = product.NegocioId,
                        NombreNegocio = product.Negocio.nombre, 
                        Codigo = product.Codigo,
                        Nombre = product.Nombre,
                        Descripcion = product.Descripcion,
                        Estado = product.Estado,
                        Iva = product.Iva,
                        RutaImagen = product.RutaImagen
                    });
                }

                var totalPages = (int)Math.Ceiling((double)products.Count / filter.PageSize);
                productosDTO = productosDTO.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToList();

                return new PagedResponse<IEnumerable<ProductoDTO>>(productosDTO, filter.PageNumber,
                    filter.PageSize, totalPages, products.Count);
            }
        }
    }
}
