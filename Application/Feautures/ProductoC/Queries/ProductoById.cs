using Application.DTOs.Productos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.PermissionS;
using Application.Specifications;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.ProductoC.Queries
{
    public class ProductoById : IRequest<ProductoDTO>
    {
        public long ProductoId { get; set; }
        public long NegocioId { get; set; }

        public required string UserId { get; set; }
    }

    public class ProductoByIdHandler : IRequestHandler<ProductoById, ProductoDTO>
    {
        private readonly IReadOnlyRepositoryAsync<Producto> _repository;
        private readonly IPermissionService _permissionService;
        public ProductoByIdHandler(IReadOnlyRepositoryAsync<Producto> repository, IPermissionService permissionService)
        {
            _repository = repository;
            _permissionService = permissionService;
        }
        public async Task<ProductoDTO> Handle(ProductoById request, CancellationToken cancellationToken)
        {
            _permissionService.ValidateNegocioPermission(request.NegocioId, request.UserId).Wait(cancellationToken);
            var product = await _repository.FirstOrDefaultAsync(new ProductoSpecification(request.ProductoId, request.NegocioId));
            if (product == null) throw new ApiException("No se ha encontrado objeto con Id: "+request.ProductoId);

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


    public class ProductoByIdParameters
    {
        public long ProductoId { get; set; }
        public long NegocioId { get; set; }
    }


}
