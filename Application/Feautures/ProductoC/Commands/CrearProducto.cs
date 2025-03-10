﻿using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Producto;
using MediatR;

namespace Application.Feautures.ProductoC.Commands
{
    public class CrearProducto : IRequest<Response<long>>
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; }
        public decimal Iva { get; set; }
        public string RutaImagen { get; set; }

        //Relaciones
        public int CategoriaId { get; set; }
        public int NegocioId { get; set; }

        //Data de Stock
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Cantidad { get; set; }

        public DateOnly FechaElaboracion { get; set; }
        public DateOnly FechaCaducidad { get; set; }
        public DateTime FechaIngreso { get; set; }

        public class CrearProductoHandler: IRequestHandler<CrearProducto, Response<long>>
        {
            private readonly IRepositoryAsync<Producto> _repository;
            private readonly IRepositoryAsync<Stock> _stockRepository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _categoriaRepository;
            private readonly IRepositoryAsync<Domain.Entities.Negocio> _negocioRepository;

            public CrearProductoHandler(IRepositoryAsync<Producto> repository, IRepositoryAsync<Stock> stockRepository, IRepositoryAsync<Domain.Entities.Categoria> categoriaRepository, IRepositoryAsync<Domain.Entities.Negocio> negocioRepository)
            {
                _repository = repository;
                _stockRepository = stockRepository;
                _categoriaRepository = categoriaRepository;
                _negocioRepository = negocioRepository;
            }

            public async Task<Response<long>> Handle(CrearProducto request, CancellationToken cancellationToken)
            {
                var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId);
                if (categoria == null)
                    throw new ApiException($"Categoría con Id {request.CategoriaId} no encontrada");

                var negocio = await _negocioRepository.GetByIdAsync(request.NegocioId);
                if (negocio == null)
                    throw new ApiException($"Negocio con Id {request.NegocioId} no encontrado");

                var producto = new Producto
                {
                    Codigo = request.Codigo,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Estado = request.Estado,
                    Iva = request.Iva,
                    RutaImagen = request.RutaImagen,
                    CategoriaId = request.CategoriaId,
                    NegocioId = request.NegocioId
                };

                await _repository.AddAsync(producto);
                await _repository.SaveChangesAsync(); 

                var stock = new Stock
                {
                    PrecioCompra = request.PrecioCompra,
                    PrecioVenta = request.PrecioVenta,
                    Cantidad = request.Cantidad,
                    FechaElaboracion = request.FechaElaboracion,
                    FechaCaducidad = request.FechaCaducidad,
                    FechaIngreso = request.FechaIngreso,
                    ProductoId = producto.Id
                };

                await _stockRepository.AddAsync(stock);
                await _stockRepository.SaveChangesAsync();

                producto.StockId = stock.Id;
                await _repository.UpdateAsync(producto);
                await _repository.SaveChangesAsync(); 

                return new Response<long>(producto.Id);
            }

        }
    }
}
