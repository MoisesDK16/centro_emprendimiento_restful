using Application.DTOs.Stats.Ganancias;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Ganancias
{
    public class GananciasPorProductos : IRequest<Response<List<GananciasPorProductoDTO>>>
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public required long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public required long CategoriaId { get; set; }
    }

    public class GananciasPorProductosHandler : IRequestHandler<GananciasPorProductos, Response<List<GananciasPorProductoDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _detalleRepository;

        public GananciasPorProductosHandler(IReadOnlyRepositoryAsync<Detalle> detalleRepository)
        {
            _detalleRepository = detalleRepository;
        }

        public async Task<Response<List<GananciasPorProductoDTO>>> Handle(GananciasPorProductos request, CancellationToken cancellationToken)
        {
            var detalles = await _detalleRepository.ListAsync(new DetalleSpecification(
                request.PageNumber, request.PageSize, request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));

            Console.WriteLine("detalles extraidos: "+ detalles.Count);


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

            return new Response<List<GananciasPorProductoDTO>>(gananciasPorProducto);
        }
    }

    public class GananciasPorProductosParameters : RequestParameter
    {
        public required long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public required long CategoriaId { get; set; }
    }

}
