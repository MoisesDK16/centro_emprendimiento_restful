using Application.DTOs.Stats.Stock.Min_Max;
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

namespace Application.Feautures.StatsC.Sock.Min_Max
{
    public class EstadoInventario : IRequest<Response<List<StockInfoDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required long NegocioId { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }

        public long? CategoriaId { get; set; }
    }

    public class EstadoInventarioHandler : IRequestHandler<EstadoInventario, Response<List<StockInfoDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _repository;
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;

        public EstadoInventarioHandler(IReadOnlyRepositoryAsync<Detalle> repository, IReadOnlyRepositoryAsync<Stock> stockRepository)
        {
            _repository = repository;
            _stockRepository = stockRepository;
        }

        public async Task<Response<List<StockInfoDTO>>> Handle(EstadoInventario request, CancellationToken cancellationToken)
        {
            var detalles = await _repository.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));

            // Obtener los stocks de los productos vendidos
            var stockList = await _stockRepository.ListAsync(new StockSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));

            Console.WriteLine("stockList: " + stockList.Count);   

            var productos = detalles
                .GroupBy(d => d.ProductoId)
                .Select(g =>
                {
                    var producto = g.First().Producto;
                    var stock = stockList.FirstOrDefault(s => s.Producto.Id == producto.Id);
                    int ventasTotales = g.Sum(d => d.Cantidad);
                    double promedio12 = ventasTotales / 12.0;
                    double mesesInvt = stock != null && promedio12 > 0 ? stock.Cantidad / promedio12 : 0;

                    int stockMin = (int)Math.Ceiling(promedio12 * 1); // 1 mes de ventas
                    int stockMax = (int)Math.Ceiling(promedio12 * 6); // 6 meses de ventas
                    int puntoPedido = (int)Math.Ceiling(promedio12 * 2); // 2 meses de ventas

                    string estadoInvt = "Normal";
                    if (stock?.Cantidad < stockMin)
                        estadoInvt = "Faltante";
                    else if (stock?.Cantidad > stockMax)
                        estadoInvt = "Exceso";

                    double cantidadAPedir = puntoPedido - (stock?.Cantidad ?? 0); // Puede ser negativa si hay exceso

                    return new StockInfoDTO
                    {
                        ProductoNombre = producto.Nombre,
                        Existencias = stock?.Cantidad ?? 0,
                        Venta = ventasTotales,
                        Promedio12 = promedio12,
                        MesesInvt = mesesInvt,
                        EstatusInvt = estadoInvt,
                        StockMin = stockMin,
                        StockMax = stockMax,
                        PuntoPedido = puntoPedido,
                        CantidadAPedir = cantidadAPedir
                    };
                })
                .ToList();

            var TotalRecords = productos.Count;
            var totalPages = (int)Math.Ceiling((double)productos.Count / request.PageSize);
            productos.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            return new PagedResponse<List<StockInfoDTO>>(productos, request.PageNumber, request.PageSize, totalPages, TotalRecords);
        }

    }

    public class EstadoInventarioParameters : RequestParameter
    {
        public required long NegocioId { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }

        public long? CategoriaId { get; set; }
    }

}
