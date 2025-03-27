using Application.DTOs.Stats.Ganancias;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Ganancias
{
    public class GananciasPorCategoria : IRequest<PagedResponse<List<GananciasPorCategoriaDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

        public long? CategoriaId { get; set; }
    }

    public class GananciasPorCategoriaHandler : IRequestHandler<GananciasPorCategoria, PagedResponse<List<GananciasPorCategoriaDTO>>>
    {

        private readonly IReadOnlyRepositoryAsync<Detalle> _repositoryDetalle;

        private readonly IReadOnlyRepositoryAsync<Producto> _productoRepository;

        public GananciasPorCategoriaHandler(IReadOnlyRepositoryAsync<Detalle> repositoryDetalle, IReadOnlyRepositoryAsync<Producto> productoRepository)
        {
            _repositoryDetalle = repositoryDetalle;
            _productoRepository = productoRepository;
        }

        public async Task<PagedResponse<List<GananciasPorCategoriaDTO>>> Handle(GananciasPorCategoria request, CancellationToken cancellationToken)
        {
            var detalles = await _repositoryDetalle.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));

            var productos = await _productoRepository.ListAsync(new ProductoSpecification(request.NegocioId, request.CategoriaId));   

            var gananciasPorProducto = detalles.Where(d => productos.Any(p => p.Id == d.ProductoId))
                .GroupBy(d => d.Producto.Categoria.Nombre).Select(d => new GananciasPorCategoriaDTO
                {
                    CantidadVendida = d.Sum(d => d.Cantidad),
                    Categoria = d.First().Producto.Categoria.Nombre,
                    CostoTotal = d.Sum(d => d.Stock.PrecioCompra * d.Cantidad),
                    Ventas = d.Sum(d => d.Precio * d.Cantidad),
                    Ganancias = d.Sum(d => (d.Precio - d.Stock.PrecioCompra) * d.Cantidad)
                }).ToList();

            var TotalRecords = gananciasPorProducto.Count;
            var TotalPages = (int)Math.Ceiling(TotalRecords / (double)request.PageSize);
            gananciasPorProducto.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            return new PagedResponse<List<GananciasPorCategoriaDTO>>(gananciasPorProducto, request.PageNumber, request.PageSize, TotalPages, TotalRecords);
        }

        public class GananciasPorCategoriaParameters : RequestParameter
        {
            public long NegocioId { get; set; }
            public DateOnly FechaInicio { get; set; }
            public DateOnly FechaFin { get; set; }

            public long? CategoriaId { get; set; }
        }
    }
}
