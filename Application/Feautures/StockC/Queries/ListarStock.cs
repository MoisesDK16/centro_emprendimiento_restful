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
    public class ListarStock: IRequest<PagedResponse<IEnumerable<Stock>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long? ProductoId { get; set; } 
        public required long NegocioId { get; set; }
        public int? Cantidad { get; set; } 
        public DateOnly? FechaCaducidad { get; set; }

        public class ListarStockHandler : IRequestHandler<ListarStock, PagedResponse<IEnumerable<Stock>>>
        {
            private readonly IReadOnlyRepositoryAsync<Stock> _repository;

            public ListarStockHandler(IReadOnlyRepositoryAsync<Stock> repository)
            {
                _repository = repository;
            }

            public async Task<PagedResponse<IEnumerable<Stock>>> Handle(ListarStock filter, CancellationToken cancellationToken)
            {
                if (filter.NegocioId == 0) throw new ApiException("NegocioId no puede ser 0");

                var stockFiltered = await _repository.ListAsync(
                    new StockSpecification(
                        filter.NegocioId,
                        filter.ProductoId,
                        filter.Cantidad,
                        filter.FechaCaducidad
                    )
                );

                var TotalElements = stockFiltered.Count;
                var TotalPages = (int)Math.Ceiling((double)stockFiltered.Count / filter.PageSize);
                stockFiltered.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize);

                return new PagedResponse<IEnumerable<Stock>>(stockFiltered, filter.PageNumber, filter.PageSize, TotalPages, TotalElements);
            }
        }
    }
}
