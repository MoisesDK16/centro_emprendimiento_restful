using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.ParametrosC.Commands
{
    public class ActualizarParametro : IRequest<Response<long>>
    {
        public long Id { get; set; }
        public required string Nombre { get; set; }
        public decimal Valor { get; set; }
    }

    public class ActualizarParametroHandler : IRequestHandler<ActualizarParametro, Response<long>>
    {
        private readonly IRepositoryAsync<Parametros> _repository;
        public ActualizarParametroHandler(IRepositoryAsync<Parametros> repository)
        {
            _repository = repository;
        }
        public async Task<Response<long>> Handle(ActualizarParametro request, CancellationToken cancellationToken)
        {
            var parametro = await _repository.GetByIdAsync(request.Id) ?? throw new ApiException($"Parametro con ID {request.Id} no encontrado.");
            parametro.Nombre = request.Nombre;
            parametro.Valor = request.Valor;
            await _repository.UpdateAsync(parametro);
            await _repository.SaveChangesAsync();
            return new Response<long>(parametro.Id);
        }
    }
}
