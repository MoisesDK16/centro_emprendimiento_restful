using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Proveedores.Commands.DeleteProveedorCommand
{
    public class DeleteProveedorCommand : IRequest<Response<string>>
    {
        public long Id { get; set; }
    }

    public class DeleteProveedorCommandHandler : IRequestHandler<DeleteProveedorCommand, Response<string>>
    {
        private readonly IRepositoryAsync<Proveedor> _repository;

        public DeleteProveedorCommandHandler(IRepositoryAsync<Proveedor> repository)
        {
            _repository = repository;
        }

        public async Task<Response<string>> Handle(DeleteProveedorCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine($"🔍 Buscando proveedor con ID {request.Id}");
            var proveedor = await _repository.GetByIdAsync(request.Id);

            if (proveedor == null) throw new ApiException($"❌ Proveedor no encontrado con el ID: {request.Id}");

            await _repository.DeleteAsync(proveedor);
            int cambios = await _repository.SaveChangesAsync();

            if (cambios > 0) return new Response<string>("✅ Registro eliminado");
            else throw new ApiException("❌ Error al tratar de eliminar el proveedor");
        }

    }
}
