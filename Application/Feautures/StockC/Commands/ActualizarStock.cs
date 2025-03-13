using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StockC.Commands
{
    public class ActualizarStock : IRequest<Response<long>>
    {
        public required long Id { get; set; }
        public required decimal PrecioCompra { get; set; }
        public required decimal PrecioVenta { get; set; }
        public required int Cantidad { get; set; }
        public required DateOnly FechaElaboracion { get; set; }
        public required DateOnly FechaCaducidad { get; set; }
        public required DateTime FechaIngreso { get; set; }

        public class ActualizarStockHandler : IRequestHandler<ActualizarStock, Response<long>>
        {
            private readonly IRepositoryAsync<Stock> _repository;

            public ActualizarStockHandler(IRepositoryAsync<Stock> repository)
            {
                _repository = repository;
            }

            public async Task<Response<long>> Handle(ActualizarStock request, CancellationToken cancellationToken)
            {
                var stockFound = await _repository.GetByIdAsync(request.Id) ?? throw new ApiException($"Stock no encontrado con el ID: {request.Id}");

                stockFound.PrecioCompra = request.PrecioCompra;
                stockFound.PrecioVenta = request.PrecioVenta;
                stockFound.Cantidad = request.Cantidad;
                stockFound.FechaElaboracion = request.FechaElaboracion;
                stockFound.FechaCaducidad = request.FechaCaducidad;
                stockFound.FechaIngreso = request.FechaIngreso;

                await _repository.UpdateAsync(stockFound);
                await _repository.SaveChangesAsync();
                return new Response<long>(stockFound.Id);
            }
        }

    }
}
