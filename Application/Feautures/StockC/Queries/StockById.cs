using Application.Exceptions;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StockC.Queries
{
    public class StockById : IRequest<Response<Stock>>
    {
        public long Id { get; set; }
    }

    public class StockByIdHandler : IRequestHandler<StockById, Response<Stock>>
    {
        private readonly IReadOnlyRepositoryAsync<Stock> _repository;
        public StockByIdHandler(IReadOnlyRepositoryAsync<Stock> repository)
        {
            _repository = repository;
        }
        public async Task<Response<Stock>> Handle(StockById request, CancellationToken cancellationToken)
        {
            var stock = await _repository.FirstOrDefaultAsync(new StockSpecification(request.Id)) 
                ?? throw new ApiException($"Stock con ID {request.Id} no encontrado.");

            return new Response<Stock>(stock);
        }
    }
}
