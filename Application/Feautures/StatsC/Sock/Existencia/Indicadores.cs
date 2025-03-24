using Application.DTOs.Stats.Stock.Existencia;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Humanizer;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Sock.Existencia
{
    public class Indicadores : IRequest<Response<List<IndicadoresDTO>>>
    {
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

    }

    public class IndicadoresHandler : IRequestHandler<Indicadores, Response<List<IndicadoresDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Historial_Stock> _historialRepository;
        private readonly IReadOnlyRepositoryAsync<Stock> _stockRepository;
        private readonly IReadOnlyRepositoryAsync<Detalle> _detalleRepository;

        public IndicadoresHandler(
            IReadOnlyRepositoryAsync<Historial_Stock> historialRepository,
            IReadOnlyRepositoryAsync<Stock> stockRepository,
            IReadOnlyRepositoryAsync<Detalle> detalleRepository)
        {
            _historialRepository = historialRepository;
            _stockRepository = stockRepository;
            _detalleRepository = detalleRepository;
        }

        public async Task<Response<List<IndicadoresDTO>>> Handle(Indicadores request, CancellationToken cancellationToken)
        {
            var stockInicial = await _historialRepository.ListAsync(
                new HistorialStockSpecification(request.NegocioId, request.FechaInicio, true));

            var existenciaInicial = stockInicial.Sum(s => s.Existencias);
            var costoInicial = stockInicial.Sum(s => s.CostoTotal);

            var entradas = await _stockRepository.ListAsync(new StockSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));

            var cantidadEntrada = entradas.Sum(e => e.Cantidad);
            var costoEntrada = entradas.Sum(e => e.Cantidad * e.PrecioCompra);

            var detalles = await _detalleRepository.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));

            var cantidadSalida = detalles.Sum(d => d.Cantidad);
            var ingresosSalida = detalles.Sum(d => d.Total);

            var stockFinal = await _historialRepository.ListAsync(new HistorialStockSpecification(request.NegocioId, request.FechaFin));

            var existenciaFinal = stockFinal.Sum(s => s.Existencias);
            var costoFinal = stockFinal.Sum(s => s.CostoTotal);


            var stockActual = await _stockRepository.ListAsync();

            var cantidadExistente = stockActual.Sum(s => s.Cantidad);
            var costoExistente = stockActual.Sum(s => s.Cantidad * s.PrecioCompra);

            var result = new IndicadoresDTO
            {
                ExistenciaInicial = existenciaInicial,
                CostoInicial = costoInicial,
                CantidadEntrada = cantidadEntrada,
                CostoEntrada = costoEntrada,
                CantidadSalida = cantidadSalida,
                IngresosSalida = ingresosSalida,
                CantidadExistente = cantidadExistente,
                CostoExistente = costoExistente,
                CantidadFinal = existenciaFinal,
                CostoFinal = costoFinal
            };

            return new Response<List<IndicadoresDTO>>(new List<IndicadoresDTO> { result });
        }
    }

}
