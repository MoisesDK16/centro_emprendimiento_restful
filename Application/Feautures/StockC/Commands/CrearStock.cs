using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.StockC.Commands
{
    public class CrearStock : IRequest<Response<long>>
    {
        public required long ProductoId { get; set; }
        public required decimal PrecioCompra { get; set; }
        public required decimal PrecioVenta { get; set; }
        public required int Cantidad { get; set; }
        public required DateOnly FechaElaboracion { get; set; }
        public required DateOnly FechaCaducidad { get; set; }
        public required DateTime FechaIngreso { get; set; }

        public class CrearStockCommandHandler : IRequestHandler<CrearStock, Response<long>>
        {
            private readonly IRepositoryAsync<Stock> _repository;
            private readonly IReadOnlyRepositoryAsync<Producto> _repositoryReadingProducto;


            public CrearStockCommandHandler(IRepositoryAsync<Stock> repository, IReadOnlyRepositoryAsync<Producto> repositoryReadingProducto)
            {
                _repository = repository;
                _repositoryReadingProducto = repositoryReadingProducto;
            }

            public async Task<Response<long>> Handle(CrearStock request, CancellationToken cancellationToken)
            {
                  var productoExiste = await _repositoryReadingProducto.GetByIdAsync(request.ProductoId) ?? throw new ApiException($"Producto no encontrado con el ID: {request.ProductoId}");
                  var stockEntity = new Stock
                   {
                       ProductoId = request.ProductoId,  
                       PrecioCompra = request.PrecioCompra,
                       PrecioVenta = request.PrecioVenta,
                       Cantidad = request.Cantidad,
                       FechaElaboracion = request.FechaElaboracion,
                       FechaCaducidad = request.FechaCaducidad,
                       FechaIngreso = request.FechaIngreso
                   };

                   var stockSaved = await _repository.AddAsync(stockEntity);
                   await _repository.SaveChangesAsync();

                   return new Response<long>(stockSaved.Id);
            }

        }
    }
}
