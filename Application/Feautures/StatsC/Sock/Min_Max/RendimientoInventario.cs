using Application.DTOs.Stats.Stock.Min_Max;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Sock.Min_Max
{
    public class RendimientoInventario : IRequest<PagedResponse<List<RendimientoInventarioDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }

    }

    public class RendimientoInventarioHandler : IRequestHandler<RendimientoInventario, PagedResponse<List<RendimientoInventarioDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _repostoryHistorical;
        private readonly IReadOnlyRepositoryAsync<Detalle> _repositoryDetalle;
        private readonly IReadOnlyRepositoryAsync<Producto> _productoRepository;

        public RendimientoInventarioHandler(
            IReadOnlyRepositoryAsync<Historial_Stock> repostoryHistorical,
            IReadOnlyRepositoryAsync<Detalle> repositoryDetalle,
            IReadOnlyRepositoryAsync<Producto> productoRepository)
        {
            _repostoryHistorical = repostoryHistorical;
            _repositoryDetalle = repositoryDetalle;
            _productoRepository = productoRepository;
        }

        public async Task<PagedResponse<List<RendimientoInventarioDTO>>> Handle(RendimientoInventario request, CancellationToken cancellationToken)
        {
            var productos = await _productoRepository.ListAsync(new ProductoSpecification(request.NegocioId, request.CategoriaId));
            Console.WriteLine("productos: " + productos.Count);

            var historial = await _repostoryHistorical.ListAsync(new HistorialStockSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));
            
            Console.WriteLine("historial: " + historial.Count);
            var detalles = await _repositoryDetalle.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));

            var rendimientos = historial.GroupBy(h => h.ProductoId).Select(g =>
            {
                var producto = productos.FirstOrDefault(p => p.Id == g.Key);
                if (producto == null) return null; 

                var invInicial = g.OrderBy(h => h.FechaInicio).First();
                var invFinal = g.OrderByDescending(h => h.FechaCorte).First();

                var ventas = detalles.Where(d => d.ProductoId == g.Key).Sum(d => d.Total);
                var ganancias = detalles.Where(d => d.ProductoId == g.Key).Sum(d => d.Precio - (d.Producto.Stocks.Where(s => s.ProductoId == producto.Id).Select(s => s.PrecioCompra)).Sum());

                var roi = invInicial.CostoTotal > 0 ? Math.Round((ganancias / invInicial.CostoTotal) * 100, 2) : 0;

                return new RendimientoInventarioDTO
                {
                    ProductoNombre = producto.Nombre,
                    InvInicial = invInicial.Existencias,    
                    costoInvInicial = invInicial.CostoTotal,
                    InvFinal = invFinal.Existencias,
                    TotalVentas = ventas,
                    ROI = roi
                };
            }).Where(x => x != null).ToList();

            var totalRecords = rendimientos.Count;
            var totalPages = (int)Math.Ceiling((double)rendimientos.Count / request.PageSize);
            rendimientos.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize); 


            return new PagedResponse<List<RendimientoInventarioDTO>>(rendimientos, request.PageNumber, request.PageSize, totalPages, totalRecords);
        }
    }

    public class RendimientoInventarioParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public long CategoriaId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }
}
