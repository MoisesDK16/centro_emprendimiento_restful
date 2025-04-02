using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.ParametrosC.Queries
{
    public class ListarParametros : IRequest<Response<List<Parametros>>>
    {
    }

    public class ListarParametrosHandler : IRequestHandler<ListarParametros, Response<List<Parametros>>>
    {
        private readonly IReadOnlyRepositoryAsync<Parametros> _repository;
        public ListarParametrosHandler(IReadOnlyRepositoryAsync<Parametros> repository)
        {
            _repository = repository;
        }
        public async Task<Response<List<Parametros>>> Handle(ListarParametros request, CancellationToken cancellationToken)
        {
            var parametros = await _repository.ListAsync();
            return new Response<List<Parametros>>(parametros);
        }
    }
}
