using Application.DTOs.Correos;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.ExternalS;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Negocio;
using MediatR;

namespace Application.Feautures.NegocioC.Commands
{
     public class CrearNegocio : IRequest<Response<long>>
    {
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public long CategoriaId { get; set; }
        public required string EmprendedorId { get; set; }
        public required string UserId { get; set; } // ID del usuario que crea el negocio, puede ser un admin o el emprendedor 

        public class CrearNegocioCommandHandler : IRequestHandler<CrearNegocio, Response<long>>
        {
            private readonly IRepositoryAsync<Negocio> _repository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repositoryCategoria;
            private readonly IUserService _userService;
            private readonly IReadOnlyRepositoryAsync<Negocio> _repositoryNegocio;
            private readonly IAccountService _accountService;
            private readonly IUnitOfWork unitOfWork;

            public CrearNegocioCommandHandler(
                IRepositoryAsync<Negocio> repository,
                IRepositoryAsync<Domain.Entities.Categoria> repositoryCategoria,
                IUserService userService,
                IReadOnlyRepositoryAsync<Negocio> repositoryNegocio,
                IAccountService accountService,
                IUnitOfWork unitOfWork)
            {
                _repository = repository;
                _repositoryCategoria = repositoryCategoria;
                _userService = userService;
                _repositoryNegocio = repositoryNegocio;
                _accountService = accountService;
                this.unitOfWork = unitOfWork;

            }

            public async Task<Response<long>> Handle(CrearNegocio request, CancellationToken cancellationToken)
            {
                await unitOfWork.BeginTransactionAsync();

                try
                {
                    bool isAdmin = false;

                    if (!string.IsNullOrEmpty(request.EmprendedorId))
                    {
                        var userExists = await _userService.UserExistsAsync(request.EmprendedorId);
                        if (!userExists)
                            throw new ApiException($"Emprendedor con id {request.EmprendedorId} no encontrado");

                        isAdmin = await _userService.IsAdmin(request.UserId);
                    }

                    var negocioExists = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Nombre, null));
                    if (negocioExists != null)
                        throw new ApiException($"Negocio con nombre {request.Nombre} ya existe");

                    var negocioExistsByTelefono = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Telefono));
                    if (negocioExistsByTelefono != null)
                        throw new ApiException($"Negocio con teléfono {request.Telefono} ya existe");

                    if (request.CategoriaId != 0)
                    {
                        var categoriaExists = await _repositoryCategoria.GetByIdAsync(request.CategoriaId);
                        if (categoriaExists == null)
                            throw new ApiException($"Categoría con id {request.CategoriaId} no encontrada");
                    }

                    var estadoNegocio = isAdmin ? Estado.Activo : Estado.Pendiente;

                    var negocio = new Negocio
                    {
                        nombre = request.Nombre,
                        direccion = request.Direccion,
                        telefono = request.Telefono,
                        estado = estadoNegocio,
                        descripcion = request.Descripcion,
                        CategoriaId = request.CategoriaId,
                        EmprendedorId = request.EmprendedorId
                    };

                    var negocioGuardado = await _repository.AddAsync(negocio);
                    await unitOfWork.SaveChangesAsync();

                    if (!isAdmin && !string.IsNullOrEmpty(request.EmprendedorId))
                    {
                        await _accountService.EnviarCorreoConfirmacionAsync(request.EmprendedorId);
                        await _accountService.EnviarSolicitudAprobacionNegocioAsync(request.EmprendedorId, negocio);
                    }

                    await unitOfWork.CommitAsync();

                    return new Response<long>(
                        negocioGuardado.Id,
                        isAdmin ? "Negocio creado y activado" : "Negocio creado. Pendiente de aprobación"
                    );
                }
                catch (Exception ex)
                {
                    await unitOfWork.RollbackAsync();
                    throw new ApiException($"Error al registrar el negocio: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

        }

        public class CrearNegocioParameters
        {
            public required string Nombre { get; set; }
            public string? Descripcion { get; set; }
            public required string Direccion { get; set; }
            public required string Telefono { get; set; }
            public long CategoriaId { get; set; }
            public required string EmprendedorId { get; set; }
        }
    }
}
    
