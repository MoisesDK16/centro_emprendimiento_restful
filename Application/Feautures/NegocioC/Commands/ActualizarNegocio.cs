using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using Domain.Enums.Negocio;
using MediatR;

namespace Application.Feautures.NegocioC.Commands
{
    public class ActualizarNegocio : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public Tipo Tipo { get; set; }
        public Estado Estado { get; set; }
        public long CategoriaId { get; set; }
        public required string UserId { get; set; }
        public class ActualizarNegocioHandler : IRequestHandler<ActualizarNegocio, Response<long>>
        {
            private readonly IRepositoryAsync<Negocio> _repository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _categoryRepository;
            private readonly IReadOnlyRepositoryAsync<Negocio> _repositoryNegocio;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IUserService _userService;
            public ActualizarNegocioHandler(
                IRepositoryAsync<Negocio> repository,
                IRepositoryAsync<Domain.Entities.Categoria> categoryRepository,
                IReadOnlyRepositoryAsync<Negocio> repositoryNegocio,
                IUnitOfWork unitOfWork,
                IUserService userService)
            {
                _repository = repository;
                _categoryRepository = categoryRepository;
                _repositoryNegocio = repositoryNegocio;
                _unitOfWork = unitOfWork;
                _userService = userService;
            }

            public async Task<Response<long>> Handle(ActualizarNegocio request, CancellationToken cancellationToken)
            {
                await _unitOfWork.BeginTransactionAsync(); 

                try
                {
                    var negocio = await _repository.GetByIdAsync(request.Id) ?? throw new ApiException($"Negocio con ID {request.Id} no encontrado.");
                    var negocioExists = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Nombre));
                    
                    if (negocioExists != null && negocioExists.Id != request.Id)
                        throw new ApiException($"Negocio con nombre {request.Nombre} ya existe");

                    var negocioExistsByTelefono = await _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Telefono));
                    if (negocioExistsByTelefono != null && negocioExistsByTelefono.Id != request.Id)
                        throw new ApiException($"Negocio con teléfono {request.Telefono} ya existe");

                    negocio.nombre = request.Nombre;
                    negocio.direccion = request.Direccion;
                    negocio.telefono = request.Telefono;
                    if (request.Estado != 0 && await _userService.IsAdmin(request.UserId)) negocio.estado = request.Estado;
                    else if (request.Estado != 0 && !await _userService.IsAdmin(request.UserId)) throw new ApiException("No tienes permisos para cambiar el estado del negocio.");

                    if (request.Descripcion != null) negocio.descripcion = request.Descripcion;

                    if (request.CategoriaId != 0)
                    {
                        ValidateCategory(request.CategoriaId);
                        negocio.CategoriaId = request.CategoriaId;
                    }

                    await _repository.UpdateAsync(negocio);
                    await _repository.SaveChangesAsync();

                    await _unitOfWork.CommitAsync(); 
                    return new Response<long>(negocio.Id);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackAsync(); 
                    throw new ApiException("Error al actualizar el negocio: " + ex.Message);
                }
            }


            private void ValidateCategory(long id)
            {
                var categoriaExists = _categoryRepository.GetByIdAsync(id) ?? throw new ApiException($"Categoria con id {id} no encontrada");
            }
        }

        public class ActualizarNegocioParameters
        {
            public long Id { get; set; }
            public required string Nombre { get; set; }
            public required string Descripcion { get; set; }
            public required string Direccion { get; set; }
            public required string Telefono { get; set; }
            public Tipo Tipo { get; set; }
            public Estado Estado { get; set; }
            public long CategoriaId { get; set; }
        }
    }
}

