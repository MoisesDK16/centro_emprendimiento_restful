using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;


namespace Application.Feautures.ProductoC.Commands
{
    public class ActualizarProducto: IRequest<Response<long>>
    {
        public long Id { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public decimal Iva { get; set; }
        public required string RutaImagen { get; set; }
        //Relaciones
        public long CategoriaId { get; set; }
        //Data de Stock
        public long StockId { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }
        public DateOnly FechaElaboracion { get; set; }
        public DateOnly FechaCaducidad { get; set; }
        public DateTime FechaIngreso { get; set; }


        public class ActualizarProductoHandler: IRequestHandler<ActualizarProducto, Response<long>>
        {
            private readonly IRepositoryAsync<Producto> _repository;
            private readonly IRepositoryAsync<Stock> _stockRepository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _categoriaRepository;

            public ActualizarProductoHandler(IRepositoryAsync<Producto> repository, IRepositoryAsync<Stock> stockRepository, IRepositoryAsync<Domain.Entities.Categoria> categoriaRepository)
            {
                _repository = repository;
                _stockRepository = stockRepository;
                _categoriaRepository = categoriaRepository;
            }
            public async Task<Response<long>> Handle(ActualizarProducto request, CancellationToken cancellationToken)
            {
                var productFound = await _repository.GetByIdAsync(request.Id)
                    ?? throw new ApiException($"Producto con Id {request.Id} no encontrado");

                _ = await _categoriaRepository.GetByIdAsync(request.CategoriaId)
                    ?? throw new ApiException($"Categoría con Id {request.CategoriaId} no encontrada");

                var stockFound = await _stockRepository.GetByIdAsync(request.StockId)
                    ?? throw new ApiException($"Stock con Id {request.StockId} no encontrado");

                productFound.Codigo = request.Codigo;
                productFound.Nombre = request.Nombre;
                productFound.Descripcion = request.Descripcion;
                productFound.Iva = request.Iva;
                productFound.RutaImagen = request.RutaImagen;
                productFound.CategoriaId = request.CategoriaId;
                productFound.StockId = request.StockId;

                productFound.Stock = stockFound;

                stockFound.PrecioCompra = request.PrecioCompra;
                stockFound.PrecioVenta = request.PrecioVenta;
                stockFound.Cantidad = request.Cantidad;
                stockFound.FechaElaboracion = request.FechaElaboracion;
                stockFound.FechaCaducidad = request.FechaCaducidad;
                stockFound.FechaIngreso = request.FechaIngreso;

                await _repository.UpdateAsync(productFound);
                await _stockRepository.UpdateAsync(stockFound);
                await _repository.SaveChangesAsync();

                return new Response<long>(productFound.Id);
            }

        }
    }
}
