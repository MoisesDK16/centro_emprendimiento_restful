using Application.DTOs.Ventas;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.VentaC.Queries
{
    public class ListarVentas : IRequest<PagedResponse<IEnumerable<GeneralVenta>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public required long NegocioId { get; set; }
        public string? IdentificacionCliente { get; set; }

        public required DateOnly FechaInicio { get; set; }

        public required DateOnly FechaFin { get; set; }

        public class ListarVentasHandler : IRequestHandler<ListarVentas, PagedResponse<IEnumerable<GeneralVenta>>>
        {
            private readonly IReadOnlyRepositoryAsync<Venta> _repositoryVenta;
            public ListarVentasHandler(IReadOnlyRepositoryAsync<Venta> repositoryVenta)
            {
                _repositoryVenta = repositoryVenta;
            }
            public async Task<PagedResponse<IEnumerable<GeneralVenta>>> Handle(ListarVentas request, CancellationToken cancellationToken)
            {
                var ventas = await _repositoryVenta.ListAsync(
                    new VentaSpecification(
                        request.NegocioId,
                        request.IdentificacionCliente,
                        request.FechaInicio,
                        request.FechaFin),
                    cancellationToken);
                
                var ventasDto = ventas.Select(x => new GeneralVenta
                {
                    Id = x.Id,
                    Fecha = x.Fecha,
                    Subtotal = x.Subtotal,
                    Total = x.Total,
                    ClienteId = x.ClienteId,
                    NegocioId = x.NegocioId
                });

                var TotalPages = (int)Math.Ceiling((double)ventas.Count / request.PageSize);
                ventasDto = ventasDto.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

                return new PagedResponse<IEnumerable<GeneralVenta>>(ventasDto, request.PageNumber, request.PageSize, TotalPages, ventas.Count);
            }
        }

    }
}
