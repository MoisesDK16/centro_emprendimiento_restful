using MediatR;
using Application.Wrappers;
using Application.Interfaces;
using AutoMapper;
using Application.Exceptions;
using Domain.Enums.Categoria;
using System.Diagnostics.CodeAnalysis;

namespace Application.Feautures.CategoriaC.Commands
{
    public class CrearCategoriaComando : IRequest<Response<long>>
    {
        public required string Nombre { get; set; }
        public required Tipo Tipo { get; set; }
        public required string Descripcion { get; set; }
        public long? NegocioId { get; set; }

        public class CrearCategoriaHandler : IRequestHandler<CrearCategoriaComando, Response<long>>
        {
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repository;

            public CrearCategoriaHandler(IRepositoryAsync<Domain.Entities.Categoria> repository)
            {
                _repository = repository;
            }

            public async Task<Response<long>> Handle(CrearCategoriaComando request, CancellationToken cancellationToken)
            {

                if (request.NegocioId != null && request.Tipo == Tipo.PRODUCTO)
                {
                   var categorySaved = await _repository.AddAsync(
                   new Domain.Entities.Categoria
                   {
                       Nombre = request.Nombre,
                       Tipo = request.Tipo,
                       Descripcion = request.Descripcion,
                       NegocioId = request.NegocioId
                   });

                    await _repository.SaveChangesAsync();
                    return new Response<long>(categorySaved.Id);

                }
                else if (request.NegocioId == null && request.Tipo == Tipo.NEGOCIO)
                {
                    var categorySaved = await _repository.AddAsync(
                    new Domain.Entities.Categoria
                    {
                        Nombre = request.Nombre,
                        Tipo = request.Tipo,
                        Descripcion = request.Descripcion
                    });
                    await _repository.SaveChangesAsync();
                    return new Response<long>(categorySaved.Id);
                }
                else throw new ApiException("Solo se puede registrar categorias de dos maneras: \n"+
                                            "-Una categoria de PRODUCTO asociado a un negocio \n" +
                                            "-Una categoria de tipo NEGOCIO sin asociación a un negocio");
            }

        }
    }



}
