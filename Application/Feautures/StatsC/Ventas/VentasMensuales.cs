using Application.DTOs.Stats.Ventas;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Ventas
{
    public class VentasMensuales : IRequest<Response<List<VentasMensualesDTO>>>
    {
        public required long NegocioId { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }
    }

    public class VentasMensualesHandler : IRequestHandler<VentasMensuales, Response<List<VentasMensualesDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _repository;

        public VentasMensualesHandler(IReadOnlyRepositoryAsync<Detalle> repository)
        {
            _repository = repository;
        }

        public async Task<Response<List<VentasMensualesDTO>>> Handle(VentasMensuales request, CancellationToken cancellationToken)
        {
            var detalles = await _repository.ListAsync(new DetalleSpecification(request.NegocioId,request.FechaInicio, request.FechaFin, request.CategoriaId));

            var ventasMensuales = detalles
                .GroupBy(d => new { d.Venta.Fecha.Year, d.Venta.Fecha.Month })
                .Select(g => new VentasMensualesDTO
                {
                    Anio = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalVentas = g.Sum(d => d.Total)
                })
                .OrderBy(v => v.Anio)
                .ThenBy(v => v.Mes)
                .ToList();

            return new Response<List<VentasMensualesDTO>>(ventasMensuales);
        }
    }

}
