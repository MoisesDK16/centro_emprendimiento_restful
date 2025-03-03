using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Proveedores.Commands.UpdateProveedorCommand
{
    public class UpdateProveedorCommand : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
    }

    public class UpdateProveedorCommandHandler : IRequestHandler<UpdateProveedorCommand, Response<long>>
    {
        private readonly IRepositoryAsync<Proveedor> _repository;
        private readonly IMapper _mapper;

        public UpdateProveedorCommandHandler(IRepositoryAsync<Proveedor> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<long>> Handle(UpdateProveedorCommand request, CancellationToken cancellationToken)
        {
            var proveedorExistente = await _repository.GetByIdAsync(request.Id);

            if (proveedorExistente == null) throw new ApiException($"Proveedor no encontrado con el ID: {request.Id}");

            proveedorExistente.nombre = request.nombre;
            proveedorExistente.telefono = request.telefono;
            proveedorExistente.correo = request.correo;
            proveedorExistente.direccion = request.direccion;
            proveedorExistente.ruc = request.ruc;
            await _repository.UpdateAsync(proveedorExistente);

            return new Response<long>(proveedorExistente.Id);

        }
    }
}





