using Application.DTOs.Promociones;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.PermissionS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.PromocionC.Queries
{
    public class ListarPromociones : IRequest<PagedResponse<IEnumerable<GeneralPromocion>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required long NegocioId { get; set; }
        public required string UserId { get; set; }

        public class ListarPromocionesHandler : IRequestHandler<ListarPromociones, PagedResponse<IEnumerable<GeneralPromocion>>>
        {
            private readonly IReadOnlyRepositoryAsync<Promocion> _repository;

            private readonly IRepositoryAsync<Negocio> _repositoryNegocio;

            private readonly IPermissionService _permissionService;

            public ListarPromocionesHandler(
                IReadOnlyRepositoryAsync<Promocion> repository,
                IRepositoryAsync<Negocio> repositoryNegocio,
                IPermissionService permissionService)
            {
                _repository = repository;
                _repositoryNegocio = repositoryNegocio;
                _permissionService = permissionService;
            }
            public async Task<PagedResponse<IEnumerable<GeneralPromocion>>> Handle(ListarPromociones request, CancellationToken cancellationToken)
            {
                _permissionService.ValidateNegocioPermission(request.NegocioId, request.UserId).Wait(cancellationToken);

                await _repositoryNegocio.GetByIdAsync(request.NegocioId);

                var promociones = await _repository.ListAsync(
                        new PromocionSpecification(request.NegocioId, true),
                        cancellationToken);

                var promocionesDTO = promociones.Select(p => new GeneralPromocion
                {
                    Id = p.Id,
                    Descuento = p.Descuento,
                    CantidadCompra = p.CantidadCompra,
                    CantidadGratis = p.CantidadGratis,
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    estado = p.Estado
                });

                var TotalPages = (int)Math.Ceiling((double)promociones.Count / request.PageSize);
                var paged  = promocionesDTO.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

                return new PagedResponse<IEnumerable<GeneralPromocion>>(paged, request.PageNumber, request.PageSize, TotalPages, promociones.Count);

            }
        }
    }
}
