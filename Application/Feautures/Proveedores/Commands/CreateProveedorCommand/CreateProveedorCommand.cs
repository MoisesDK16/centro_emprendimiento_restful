using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.Proveedores.Commands.CreateProveedorCommand
{
    public class CreateProveedorCommand : IRequest<Response<long>>
    {
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
    }

    public class CreateProveedorCommandHandler : IRequestHandler<CreateProveedorCommand, Response<long>>
    {
        private readonly IRepositoryAsync<Proveedor> _repository;
        private readonly IMapper _mapper;

        public CreateProveedorCommandHandler(IRepositoryAsync<Proveedor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<long>> Handle(CreateProveedorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newProveedor = _mapper.Map<Proveedor>(request);
                Console.WriteLine("Proveedor mapeado: " + newProveedor.nombre);

                var createdProveedor = await _repository.AddAsync(newProveedor);
                Console.WriteLine("Proveedor agregado a EF, esperando guardar en BD...");

                int cambios = await _repository.SaveChangesAsync();
                Console.WriteLine($"Cambios guardados en la BD: {cambios}");

                if (cambios == 0)
                {
                    Console.WriteLine("Error: No se guardaron cambios en la BD.");
                    return Response<long>.Fail("No se guardaron cambios en la BD.");
                }

                Console.WriteLine($"Proveedor guardado con ID: {createdProveedor.Id}");
                return new Response<long>(createdProveedor.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el Handler: {ex.Message}");
                return Response<long>.Fail($"Error en el Handler: {ex.Message} | InnerException: {ex.InnerException?.Message}");
            }
        }


    }
}
