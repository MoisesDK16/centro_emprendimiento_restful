using MediatR;
using Application.Wrappers;
using Application.Interfaces;
using AutoMapper;
using Application.Exceptions;
using Domain.Enums.Categoria;

namespace Application.Feautures.CategoriaC.Commands
{
    public class CrearCategoriaComando : IRequest<Response<long>>
    {
        public required string Nombre { get; set; }

        public required Tipo Tipo { get; set; }
        public required string Descripcion { get; set; }

        public class CrearCategoriaHandler : IRequestHandler<CrearCategoriaComando, Response<long>>
        {
            private readonly IRepositoryAsync<Domain.Entities.Categoria> _repository;
            private readonly IMapper _mapper;

            public CrearCategoriaHandler(IRepositoryAsync<Domain.Entities.Categoria> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<long>> Handle(CrearCategoriaComando request, CancellationToken cancellationToken)
            {
               var categorySaved = _repository.AddAsync(_mapper.Map<Domain.Entities.Categoria>(request));
               await _repository.SaveChangesAsync();
               return new Response<long>(categorySaved.Result.Id); 

            }
        }
    }



}
