using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StatsC.Cuadros_Mando
{
    public class TotalVentas : IRequest<Response<decimal>>
    {
        public long NegocioId { get; set; }
        public required DateOnly FechaInicio { get; set; }
        public required DateOnly FechaFin { get; set; }
        public long CategoriaId { get; set; }

        public class TotalVentasHandler : IRequestHandler<TotalVentas, Response<decimal>>
        {
            private readonly IReadOnlyRepositoryAsync<Detalle> _repository;
            public TotalVentasHandler(IReadOnlyRepositoryAsync<Detalle> repository)
            {
                _repository = repository;
            }

            public async Task<Response<decimal>> Handle(TotalVentas request, CancellationToken cancellationToken)
            {
                var detalles = await _repository.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin, request.CategoriaId));
                var ventas = detalles
                  .Sum(d => d.Precio * d.Cantidad);
                return new Response<decimal>(ventas);
            }
        }
    }
}
