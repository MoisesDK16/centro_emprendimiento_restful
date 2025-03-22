using Application.DTOs.Stats.Stock.Existencia;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
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

    /*public class IndicadoresHandler : IRequestHandler<Indicadores, Response<List<IndicadoresDTO>>>
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
            var inicio = request.FechaInicio.ToDateTime(new TimeOnly(0, 0));
            var fin = request.FechaFin.ToDateTime(new TimeOnly(23, 59));

            // Existencia y Costo Inicial (más cercano antes del rango)
            var stockInicial = await _historialRepository.ListAsync(h => h.NegocioId == request.NegocioId && h.FechaCorte <= inicio);

            var existenciaInicial = stockInicial.Sum(s => s.Existencias);
            var costoInicial = stockInicial.Sum(s => s.CostoTotal);

            // Entradas (compras): Stock ingresado dentro del periodo
            var entradas = await _stockRepository.ListAsync(s =>
                s.Producto.NegocioId == request.NegocioId && s.FechaIngreso >= inicio && s.FechaIngreso <= fin);

            var cantidadEntrada = entradas.Sum(e => e.Cantidad);
            var costoEntrada = entradas.Sum(e => e.Cantidad * e.PrecioCompra);

            // Salidas (ventas): Detalles en el rango
            var detalles = await _detalleRepository.ListAsync(d =>
                d.Venta.NegocioId == request.NegocioId && d.Venta.Fecha >= inicio && d.Venta.Fecha <= fin);

            var cantidadSalida = detalles.Sum(d => d.Cantidad);
            var ingresosSalida = detalles.Sum(d => d.Total);

            // Existencia y Costo Final: la última del historial
            var stockFinal = await _historialRepository.ListAsync(h =>
                h.NegocioId == request.NegocioId && h.FechaCorte <= fin);

            var existenciaFinal = stockFinal.Sum(s => s.Existencias);
            var costoFinal = stockFinal.Sum(s => s.CostoTotal);

            // Stock actual (por si deseas mostrar cantidad existente y costo a la fecha actual)
            var stockActual = await _stockRepository.ListAsync(s =>
                s.Producto.NegocioId == request.NegocioId);

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
    }*/

}
