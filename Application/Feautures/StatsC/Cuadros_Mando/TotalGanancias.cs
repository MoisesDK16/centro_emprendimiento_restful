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

namespace Application.Feautures.StatsC.Cuadros_Mando
{
    public class TotalGanancias : IRequest<Response<decimal>>
    {
        public long NegocioId { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }

        public long CategoriaId { get; set; }
    }

    public class TotalGananciasHandler : IRequestHandler<TotalGanancias, Response<decimal>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _repository;
        public TotalGananciasHandler(IReadOnlyRepositoryAsync<Detalle> repository)
        {
            _repository = repository;
        }
        public async Task<Response<decimal>> Handle(TotalGanancias request, CancellationToken cancellationToken)
        {
            var detalles = await _repository.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));

            var ganancias = detalles
              .Sum(d => (d.Precio - d.Stock.PrecioCompra) * d.Cantidad);

            return new Response<decimal>(ganancias);
        }

    }
}
