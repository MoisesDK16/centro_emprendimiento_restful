using Application.DTOs.Stats.Ganancias;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Ganancias
{
    public class GananciasPorProductos : IRequest<PagedResponse<List<GananciasPorProductoDTO>>>
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public required long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public long? CategoriaId { get; set; }
    }

    public class GananciasPorProductosHandler : IRequestHandler<GananciasPorProductos, PagedResponse<List<GananciasPorProductoDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _detalleRepository;

        public GananciasPorProductosHandler(IReadOnlyRepositoryAsync<Detalle> detalleRepository)
        {
            _detalleRepository = detalleRepository;
        }

        public async Task<PagedResponse<List<GananciasPorProductoDTO>>> Handle(GananciasPorProductos request, CancellationToken cancellationToken)
        {
            Console.WriteLine("FechaInicio: " + request.FechaInicio);
            Console.WriteLine("FechaFin: " + request.FechaFin);
            var detalles = await _detalleRepository.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));

            Console.WriteLine("DETALLES: " + detalles.Count);

            var gananciasPorProducto = detalles
                .GroupBy(d => d.Producto.Nombre)
                .Select(g => new GananciasPorProductoDTO
                {
                    Nombre = g.Key,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    CostoTotal = g.Sum(d => d.Stock.PrecioCompra * d.Cantidad),
                    GananciaTotal = g.Sum(d => (d.Precio - d.Stock.PrecioCompra) * d.Cantidad)
                })
                .OrderByDescending(p => p.GananciaTotal)
                .ToList();

            var totalPages = (int)Math.Ceiling((double)gananciasPorProducto.Count / request.PageSize);

            gananciasPorProducto.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return new PagedResponse<List<GananciasPorProductoDTO>>(gananciasPorProducto, request.PageNumber,
                request.PageSize, totalPages, gananciasPorProducto.Count);
        }
    }

    public class GananciasPorProductosParameters : RequestParameter
    {
        public required long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }
    }

}
