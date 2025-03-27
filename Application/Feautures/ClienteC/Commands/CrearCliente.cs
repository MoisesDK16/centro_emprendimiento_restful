using Application.Behaviors;
using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.ClienteC.Commands
{
    public class CrearCliente : IRequest<Response<long>>
    {
        public required string Identificacion { get; set; }
        public required string Nombres { get; set; }
        public required string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public required string Ciudad { get; set; }
        public string? Direccion { get; set; }
        public long NegocioId { get; set; }


        public class CrearClienteCommandHandler : IRequestHandler<CrearCliente, Response<long>>
        {
            private readonly IRepositoryAsync<Cliente> _repository;
            private readonly IReadOnlyRepositoryAsync<Cliente> _repositoryClienteReading;
            private readonly IRepositoryAsync<NegocioCliente> _repositoryNegocioCliente;
            private readonly IRepositoryAsync<Domain.Entities.Negocio> _repositoryNegocio;

            public CrearClienteCommandHandler(
                IRepositoryAsync<Cliente> repository,
                IRepositoryAsync<NegocioCliente> repositoryNegocioCliente,
                IRepositoryAsync<Domain.Entities.Negocio> repositoryNegocio,
                IReadOnlyRepositoryAsync<Cliente> repositoryClienteReading)
            {
                _repository = repository;
                _repositoryNegocioCliente = repositoryNegocioCliente;
                _repositoryNegocio = repositoryNegocio;
                _repositoryClienteReading = repositoryClienteReading;
            }

            public async Task<Response<long>> Handle(CrearCliente request, CancellationToken cancellationToken)
            {
                var negocio = await _repositoryNegocio.GetByIdAsync(request.NegocioId);
                if (negocio == null) throw new ApiException($"Negocio con id {request.NegocioId} no encontrado");

                if(request.Identificacion != null)
                {
                    var clienteExistente = await _repositoryClienteReading.FirstOrDefaultAsync(new ClienteSpecification(request.Identificacion));
                    if (clienteExistente != null) throw new ApiException($"Cliente con identificacion {request.Identificacion} ya existe");
                }
                if(request.Identificacion != null)
                {
                    if (!ValidacionIdentificacion.VerificaIdentificacion(request.Identificacion))
                        throw new ApiException("Identificacion no valida");

                } 

                var cliente = new Cliente
                {
                    Identificacion = request.Identificacion,
                    Nombres = request.Nombres,
                    PrimerApellido = request.PrimerApellido,
                    SegundoApellido = request.SegundoApellido,
                    Email = request.Email,
                    Telefono = request.Telefono,
                    Ciudad = request.Ciudad,
                    Direccion = request.Direccion
                };

                var clienteGuardado = await _repository.AddAsync(cliente);
                await _repository.SaveChangesAsync();

                var negocioCliente = new NegocioCliente
                {
                    ClienteId = clienteGuardado.Id,
                    NegocioId = negocio.Id
                };

                await _repositoryNegocioCliente.AddAsync(negocioCliente);
                await _repositoryNegocioCliente.SaveChangesAsync();

                return new Response<long>(clienteGuardado.Id);
            }

        }

    }
}
