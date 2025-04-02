using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Negocio;
using MediatR;

namespace Application.Feautures.NegocioC.Commands
{
     public class CrearNegocio : IRequest<Response<long>>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public Estado Estado { get; set; }
        public long Categoria { get; set; }
        public string EmprendedorId { get; set; }

        public class CrearNegocioCommandHandler : IRequestHandler<CrearNegocio, Response<long>>
        {
            private readonly IRepositoryAsync<Negocio> _repository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repositoryCategoria;
            private readonly IUserService _userService;
            private readonly IReadOnlyRepositoryAsync<Negocio> _repositoryNegocio;

            public CrearNegocioCommandHandler(
                IRepositoryAsync<Negocio> repository,
                IRepositoryAsync<Domain.Entities.Categoria> repositoryCategoria,
                IUserService userService,
                IReadOnlyRepositoryAsync<Negocio> repositoryNegocio)
            {
                _repository = repository;
                _repositoryCategoria = repositoryCategoria;
                _userService = userService;
                _repositoryNegocio = repositoryNegocio;
            }

            public async Task<Response<long>> Handle(CrearNegocio request, CancellationToken cancellationToken)
            {
                if (!string.IsNullOrEmpty(request.EmprendedorId))
                {
                    var userExists = await _userService.UserExistsAsync(request.EmprendedorId);
                    if (!userExists)
                        throw new ApiException($"Emprendedor con id {request.EmprendedorId} no encontrado");
                }

                var negocioExists = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Nombre, null));
                if (negocioExists != null)
                    throw new ApiException($"Negocio con nombre {request.Nombre} ya existe");

                var negocioExistsByTelefono = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Telefono));
                if (negocioExistsByTelefono != null)
                    throw new ApiException($"Negocio con telefono {request.Telefono} ya existe");


                if (request.Categoria != 0)
                {
                    var categoriaExists = await _repositoryCategoria.GetByIdAsync(request.Categoria);
                    if (categoriaExists == null)
                        throw new ApiException($"Categoria con id {request.Categoria} no encontrada");
                }

                // Create Negocio entity
                var negocio = new Negocio
                {
                    nombre = request.Nombre,
                    direccion = request.Direccion,
                    telefono = request.Telefono,
                    estado = request.Estado,
                    descripcion = request.Descripcion,
                    CategoriaId = request.Categoria,
                    EmprendedorId = request.EmprendedorId
                };

                // Save Negocio
                var negocioGuardado = await _repository.AddAsync(negocio);
                await _repository.SaveChangesAsync();

                return new Response<long>(negocioGuardado.Id);
            }

        }
    }
}
    
