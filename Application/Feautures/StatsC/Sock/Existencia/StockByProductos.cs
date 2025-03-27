using Application.DTOs.Stats.Stock.Existencia;
using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Sock.Existencia
{
    public class StockByProductos : IRequest<PagedResponse<List<StockReportDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NegocioId { get; set; }
        public long CategoriaId { get; set; }
    }

    public class StockByProductosHandler : IRequestHandler<StockByProductos, PagedResponse<List<StockReportDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;

        public StockByProductosHandler(IReadOnlyRepositoryAsync<Stock> stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<PagedResponse<List<StockReportDTO>>> Handle(StockByProductos request, CancellationToken cancellationToken)
        {
            var stockList = await _stockRepository.ListAsync(new StockSpecification(request.NegocioId, request.CategoriaId, true)
            );

            var productosStock = stockList
                 .GroupBy(s => s.Producto.Nombre)
                 .Distinct()
                 .Select(g => new StockReportDTO
                 {
                     Producto = g.Key,
                     Stock = g.Sum(s => s.Cantidad)
                 })
                 .OrderByDescending(p => p.Stock)
                 .ToList();

            var TotalRecords = productosStock.Count;
            var TotalPages = (int)Math.Ceiling((double)productosStock.Count / request.PageSize);
            productosStock.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return new PagedResponse<List<StockReportDTO>>(productosStock, request.PageNumber, request.PageSize, TotalPages, TotalRecords);

        }
    }

    public class StockByProductosParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public long CategoriaId { get; set; }
    }
}
