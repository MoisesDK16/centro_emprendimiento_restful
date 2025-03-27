using Application.DTOs.Stats.Stock.ABC;
using Application.Feautures.Categoria.Queries;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Sock.ABC
{
    public class Clasificacion : IRequest<PagedResponse<List<ClasificacionDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NegocioId { get; set; }
        public long categoriaId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }

    public class ClasificacionHandler : IRequestHandler<Clasificacion, PagedResponse<List<ClasificacionDTO>>>
    {

        private readonly IReadOnlyRepositoryAsync<Detalle> _repositoryDetalle;
        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _repositoryHistorial;

        public ClasificacionHandler(IReadOnlyRepositoryAsync<Detalle> repositoryDetalle, IReadOnlyRepositoryAsync<Historial_Stock> repositoryHistorial)
        {
            _repositoryDetalle = repositoryDetalle;
            _repositoryHistorial = repositoryHistorial;
        }

        public async Task<PagedResponse<List<ClasificacionDTO>>> Handle(Clasificacion request, CancellationToken cancellationToken)
        {
            var detalles = await _repositoryDetalle.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.categoriaId));
            var historial = await _repositoryHistorial.ListAsync(new HistorialStockSpecification(request.NegocioId));

            var productos = detalles
                .GroupBy(d => d.ProductoId)
                .Select(g =>
                {
                    var producto = g.First().Producto;
                    decimal totalVendido = g.Sum(d => d.Total);

                    return new
                    {
                        Producto = producto,
                        TotalVendido = totalVendido
                    };
                })
                .OrderByDescending(p => p.TotalVendido)
                .ToList(); 

            decimal totalGeneral = productos.Sum(p => p.TotalVendido);
            decimal acumulado = 0;
            decimal acumuladoPorc = 0;

            var resultado = productos.Select(p =>
            {
                decimal porcentaje = totalGeneral > 0 ? Math.Round((p.TotalVendido / totalGeneral) * 100, 2) : 0;
                acumulado += porcentaje;
                acumuladoPorc = Math.Round(acumulado, 2);

                return new ClasificacionDTO
                {
                    ProductoNombre = p.Producto.Nombre,
                    TotalVendido = p.TotalVendido,
                    Acumulado = Math.Round(acumulado, 2),
                    PorcentajeAcumulado = porcentaje,
                    ClasificacionABC = acumuladoPorc >= 30 && acumuladoPorc <= 90 ? "A" :
                                       acumuladoPorc < 5 ? "C" : "B"

                };
            }).ToList();

            var totalPages = (int)Math.Ceiling((double)resultado.Count / request.PageSize);
            var totalRecords = resultado.Count;
            resultado.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return new PagedResponse<List<ClasificacionDTO>>(resultado, request.PageNumber, request.PageSize, totalPages, totalRecords);
        }

    }

    public class ClasificacionParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public long categoriaId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
