using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace Application.Feautures.ProductoC.Commands
{
    public class ActualizarProducto: IRequest<Response<long>>
    {
        public long Id { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public decimal Iva { get; set; }
        public IFormFile? Imagen { get; set; }
        //Relaciones
        public long CategoriaId { get; set; }

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
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _categoriaRepository;
            private readonly IAzureStorageService _azureStorageService;

            public ActualizarProductoHandler(
                IRepositoryAsync<Producto> repository,
                IRepositoryAsync<Stock> stockRepository,
                IRepositoryAsync<Domain.Entities.Categoria> categoriaRepository,
                IAzureStorageService azureStorageService
                )
            {
                _repository = repository;
                _categoriaRepository = categoriaRepository;
                _azureStorageService = azureStorageService;
            }
            public async Task<Response<long>> Handle(ActualizarProducto request, CancellationToken cancellationToken)
            {
                var productFound = await _repository.GetByIdAsync(request.Id)
                    ?? throw new ApiException($"Producto con Id {request.Id} no encontrado");

                _ = await _categoriaRepository.GetByIdAsync(request.CategoriaId)
                    ?? throw new ApiException($"Categoría con Id {request.CategoriaId} no encontrada");

                string? path = null; 

                if(request.Imagen != null)
                    path = await _azureStorageService.UploadAsync(request.Imagen, ContainerEnum.IMAGES, request.Imagen.FileName);

                productFound.Codigo = request.Codigo;
                productFound.Nombre = request.Nombre;
                productFound.Descripcion = request.Descripcion;
                productFound.Iva = request.Iva;
                productFound.RutaImagen = path;
                productFound.CategoriaId = request.CategoriaId;

                await _repository.UpdateAsync(productFound);
                await _repository.SaveChangesAsync();

                return new Response<long>(productFound.Id);
            }

        }
    }
}
