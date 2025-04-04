using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Azure.Core;
using Domain.Entities;
using Domain.Enums.Negocio;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Feautures.NegocioC.Commands
{
    public class ActualizarNegocio : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public Tipo Tipo { get; set; }
        public Estado Estado { get; set; }
        public long CategoriaId { get; set; }
        public class ActualizarNegocioHandler : IRequestHandler<ActualizarNegocio, Response<long>>
        {
            private readonly IRepositoryAsync<Negocio> _repository;
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repositoryCategoria;
            private readonly IReadOnlyRepositoryAsync<Negocio> _repositoryNegocio;
            public ActualizarNegocioHandler(
                IRepositoryAsync<Negocio> repository,
                IRepositoryAsync<Domain.Entities.Categoria> repositoryCategoria,
                IReadOnlyRepositoryAsync<Negocio> repositoryNegocio)
            {
                _repository = repository;
                _repositoryCategoria = repositoryCategoria;
                _repositoryNegocio = repositoryNegocio;
            }
            public async Task<Response<long>> Handle(ActualizarNegocio request, CancellationToken cancellationToken)
            {
                var negocio = await _repository.GetByIdAsync(request.Id);
                if (negocio == null) throw new ApiException($"Negocio con ID {request.Id} no encontrado.");

                var negocioExists = _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Nombre));
                if (negocioExists != null)
                    throw new ApiException($"Negocio con nombre {request.Nombre} ya existe");

                var negocioExistsByTelefono = _repositoryNegocio.FirstOrDefaultAsync(new NegocioSpecification(request.Telefono));
                if (negocioExistsByTelefono != null)
                    throw new ApiException($"Negocio con telefono {request.Telefono} ya existe");

                if (negocio.nombre != null)
                {
                    negocio.nombre = request.Nombre;
                }
                if (negocio.descripcion != null)
                {
                    negocio.descripcion = request.Descripcion;
                }
                if (negocio.direccion != null)
                {
                    negocio.direccion = request.Direccion;
                }
                if (negocio.telefono != null)
                {
                    negocio.telefono = request.Telefono;
                }
                negocio.estado = request.Estado;

                if (request.CategoriaId != 0)
                {
                    validateCategory(request.CategoriaId);
                    negocio.CategoriaId = request.CategoriaId;
                }

                await _repository.UpdateAsync(negocio);
                await _repository.SaveChangesAsync();

                return new Response<long>(negocio.Id);
            }


            private void validateCategory(long id)
            {
                var categoriaExists =  _repositoryCategoria.GetByIdAsync(id);
                if (categoriaExists == null)
                    throw new ApiException($"Categoria con id {id} no encontrada");
            }

        }
    }
}
