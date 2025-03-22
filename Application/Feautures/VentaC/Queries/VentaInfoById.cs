using Application.DTOs.Detalles;
using Application.DTOs.Ventas;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.VentaC.Queries
{
    public class VentaInfoById : IRequest<Response<VentaInfo>>
    {
        public long Id { get; set; }
        
    }

    public class VentaInfoByIdHandler : IRequestHandler<VentaInfoById, Response<VentaInfo>>
    {

        private readonly IReadOnlyRepositoryAsync<Venta> _repository;

        public VentaInfoByIdHandler(IReadOnlyRepositoryAsync<Venta> repository)
        {
            _repository = repository;
        }

        public async Task<Response<VentaInfo>> Handle(VentaInfoById request, CancellationToken cancellationToken)
        {
            var venta = await _repository.FirstOrDefaultAsync(new VentaSpecification(request.Id));
            if (venta == null) throw new ApiException($"Venta con ID {request.Id} no encontrada.");

            return new Response<VentaInfo>(new VentaInfo
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                Total = venta.Total,
                ClienteId = venta.ClienteId,
                NegocioId = venta.NegocioId,
                Detalles = venta.Detalles.Select(d => new DetalleInfo
                {
                    Id = d.Id,
                    Precio = d.Precio,
                    Cantidad = d.Cantidad,
                    Total = d.Total,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto.Nombre,
                    PromocionId = d.PromocionId,
                    PromocionNombre = d.Promocion?.TipoPromocion.ToString()
                }).ToList()
            });
        }

    }
}
