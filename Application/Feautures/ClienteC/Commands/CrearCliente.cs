using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.ClienteC.Commands
{
    public class CrearCliente : IRequest<Response<long>>
    {
        public string? Identificacion { get; set; }
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string Ciudad { get; set; }
        public string? Direccion { get; set; }
        public long NegocioId { get; set; }


        public class CrearClienteCommandHandler : IRequestHandler<CrearCliente, Response<long>>
        {
            private readonly IRepositoryAsync<Cliente> _repository;
            private readonly IRepositoryAsync<NegocioCliente> _repositoryNegocioCliente;
            private readonly IRepositoryAsync<Domain.Entities.Negocio> _repositoryNegocio;

            public CrearClienteCommandHandler(
                IRepositoryAsync<Cliente> repository,
                IRepositoryAsync<NegocioCliente> repositoryNegocioCliente,
                IRepositoryAsync<Domain.Entities.Negocio> repositoryNegocio)
            {
                _repository = repository;
                _repositoryNegocioCliente = repositoryNegocioCliente;
                _repositoryNegocio = repositoryNegocio;
            }

            public async Task<Response<long>> Handle(CrearCliente request, CancellationToken cancellationToken)
            {
                var negocio = await _repositoryNegocio.GetByIdAsync(request.NegocioId);
                if (negocio == null) throw new ApiException($"Negocio con id {request.NegocioId} no encontrado");

                var cliente = new Cliente
                {
                    Identificacion = request.Identificacion,
                    Nombres = request.Nombres,
                    PrimerApellido = request.PrimerApellido,
                    SegundoApellido = request.SegundoApellido,
                    Email = request.Email,
                    Telefono = request.Telefono,
                    Ciudad = request.Ciudad,
                    Direccion = request.Direccion,
                    NegocioClientes = new List<NegocioCliente>() 
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
