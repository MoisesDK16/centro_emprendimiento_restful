using Application.DTOs.Stats.Ganancias;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StatsC.Ganancias
{
    public class MejoresClientes : IRequest<Response<List<MejoresClientesDTO>>>
    {
        public long NegocioId { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
    }

    public class MejoresClientesHandler : IRequestHandler<MejoresClientes, Response<List<MejoresClientesDTO>>>
    {
        private readonly IReadOnlyRepositoryAsync<Detalle> _repositoryDetalle;
        private readonly IReadOnlyRepositoryAsync<Cliente> _repositoryCliente;

        public MejoresClientesHandler(IReadOnlyRepositoryAsync<Detalle> repositoryDetalle, IReadOnlyRepositoryAsync<Cliente> repositoryCliente)
        {
            _repositoryDetalle = repositoryDetalle;
            _repositoryCliente = repositoryCliente;
        }

        public async Task<Response<List<MejoresClientesDTO>>> Handle(MejoresClientes request, CancellationToken cancellationToken)
        {
            var detalles = await _repositoryDetalle.ListAsync(new DetalleSpecification(request.NegocioId, request.FechaInicio, request.FechaFin));
            var clientes = await _repositoryCliente.ListAsync();
            var clientesConCompras = detalles.GroupBy(d => d.Venta.ClienteId).Select(d => new MejoresClientesDTO
            {
                Identificacion = clientes.First(c => c.Id == d.Key).Identificacion ?? null,
                NombreCliente = clientes.First(c => c.Id == d.Key).Nombres,
                ApellidoCliente = clientes.First(c => c.Id == d.Key).PrimerApellido,
                TotalCompras = d.Sum(d => d.Total),
                CantidadCompras = d.Count(),
                PromedioCompraMensual = Math.Truncate((d.Sum(d => d.Total) / 12) * 100) / 100,
                TotalGanancias = d.Sum(d => d.Total)

            }).OrderByDescending(c => c.TotalGanancias).Take(3).ToList();
            var paged = clientesConCompras.ToList();
            return new Response<List<MejoresClientesDTO>>(paged);
        }
    }
}
