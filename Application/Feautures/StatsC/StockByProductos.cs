using Application.Interfaces;
using Application.Parameters;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC
{
    public class StockByProductos : IRequest<PagedResponse<List<StockReportDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long NegocioId { get; set; }
        public long CategoriaId { get; set; }    
    }

       public class StockByProductosHandler : IRequestHandler<StockByProductos, PagedResponse<List<StockReportDto>>>
         {
          private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;

          public StockByProductosHandler(IReadOnlyRepositoryAsync<Stock> stockRepository)
          {
              _stockRepository = stockRepository;
          }

        public async Task<PagedResponse<List<StockReportDto>>> Handle(StockByProductos request, CancellationToken cancellationToken)
        {
            var stockList = await _stockRepository.ListAsync(new StockSpecification(
               request.PageNumber, request.PageSize, request.NegocioId, request.CategoriaId)
            );

            var productosStock = stockList
                 .GroupBy(s => s.Producto.Nombre)
                 .Distinct()
                 .Select(g => new StockReportDto
                 {
                     Producto = g.Key,
                     Stock = g.Sum(s => s.Cantidad)
                 })
                 .OrderByDescending(p => p.Stock)
                 .ToList();

            return new PagedResponse<List<StockReportDto>>(productosStock, request.PageNumber, request.PageSize);

          }
       }

        public class StockReportDto
    {
        public string Producto { get; set; }
        public int Stock { get; set; }
    }

    public class StockByProductosParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public long CategoriaId { get; set; }
    }
}
