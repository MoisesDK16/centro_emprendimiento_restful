using Application.DTOs.Productos;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ProductoC.Queries
{
    public class ProductoById : IRequest<ProductoDTO>
    {
        public required long Id { get; set; }

        public class ProductoByIdHandler : IRequestHandler<ProductoById, ProductoDTO>
        {
            private readonly IRepositoryAsync<Producto> _repository;
            public ProductoByIdHandler(IRepositoryAsync<Producto> repository)
            {
                _repository = repository;
            }
            public async Task<ProductoDTO> Handle(ProductoById request, CancellationToken cancellationToken)
            {
                var product = await _repository.GetByIdAsync(request.Id);
                if (product == null) throw new ApiException("No se ha encontrado objeto con Id: "+request.Id);
                return new ProductoDTO
                {
                    Id = product.Id,
                    CategoriaId = product.CategoriaId,
                    NombreCategoria = product.Categoria.Nombre,
                    NegocioId = product.NegocioId,
                    NombreNegocio = product.Negocio.nombre,
                    Codigo = product.Codigo,
                    Nombre = product.Nombre,
                    Descripcion = product.Descripcion,
                    Estado = product.Estado,
                    Iva = product.Iva,
                    RutaImagen = product.RutaImagen
                };
            }

        }
    }
}
