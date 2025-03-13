using Application.DTOs.Categorias;
using Application.Interfaces;
using Domain.Enums.Categoria;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.CategoriaC.Queries
{
    public class SelectCategorias : IRequest<IEnumerable<CategoriaDTO>>
    {
        public required Tipo tipo { get; set; }
        public int NegocioId { get; set; }

        public class SelectCategoriasHandler : IRequestHandler<SelectCategorias, IEnumerable<CategoriaDTO>>
        {
            private readonly IReadOnlyRepositoryAsync<Domain.Entities.Categoria> _repository;

            public SelectCategoriasHandler(IReadOnlyRepositoryAsync<Domain.Entities.Categoria> repository)
            {
                _repository = repository;
            }
            public async Task<IEnumerable<CategoriaDTO>> Handle(SelectCategorias request, CancellationToken cancellationToken)
            {
                IEnumerable<Domain.Entities.Categoria> categorias = await _repository.ListAsync();

                if(request.tipo == Tipo.PRODUCTO){
                    categorias = categorias.Where(categorias => categorias.Tipo == Tipo.PRODUCTO);
                    categorias = categorias.Where(categorias => categorias.NegocioId == request.NegocioId);
                }
                else categorias = categorias.Where(categorias => categorias.Tipo == Tipo.NEGOCIO);

                List<CategoriaDTO> categoriasDTO = new();

                categorias.ToList().ForEach(categoria =>
                {
                    categoriasDTO.Add(new CategoriaDTO
                    {
                        Id = categoria.Id,
                        nombre = categoria.Nombre
                    });
                });

                return categoriasDTO;
            }
        }

    }

}
