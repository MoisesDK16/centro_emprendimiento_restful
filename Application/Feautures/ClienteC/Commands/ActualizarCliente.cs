using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ClienteC.Commands
{
    public class ActualizarCliente : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public string? Identificacion { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string Ciudad { get; set; }
        public string? Direccion { get; set; }

        public class ActualizarClienteHandler : IRequestHandler<ActualizarCliente, Response<long>>
        {
            private readonly IRepositoryAsync<Cliente> _repository;
            private readonly IReadOnlyRepositoryAsync<Cliente> _repositoryReading;
            private readonly IRepositoryAsync<NegocioCliente> _repositoryNegocioCliente;
            private readonly IRepositoryAsync<Domain.Entities.Negocio> _repositoryNegocio;

            public ActualizarClienteHandler(
                IRepositoryAsync<Cliente> repository,
                IReadOnlyRepositoryAsync<Cliente> repositoryReading,
                IRepositoryAsync<NegocioCliente> repositoryNegocioCliente,
                IRepositoryAsync<Domain.Entities.Negocio> repositoryNegocio)
            {
                _repository = repository;
                _repositoryReading = repositoryReading;
                _repositoryNegocioCliente = repositoryNegocioCliente;
                _repositoryNegocio = repositoryNegocio;
            }

            public async Task<Response<long>> Handle(ActualizarCliente request, CancellationToken cancellationToken)
            {
                var cliente = await _repository.GetByIdAsync(request.Id);
                if (cliente == null) throw new ApiException($"liente con ID {request.Id} no encontrado.");

                cliente.Identificacion = request.Identificacion;
                cliente.Nombres = request.Nombres;
                cliente.PrimerApellido = request.PrimerApellido;
                cliente.SegundoApellido = request.SegundoApellido;
                cliente.Email = request.Email;
                cliente.Telefono = request.Telefono;
                cliente.Ciudad = request.Ciudad;
                cliente.Direccion = request.Direccion;

                await _repository.UpdateAsync(cliente);
                await _repository.SaveChangesAsync();

                await _repositoryNegocioCliente.SaveChangesAsync();

                return new Response<long>(cliente.Id);
            }
        }
    }

}
