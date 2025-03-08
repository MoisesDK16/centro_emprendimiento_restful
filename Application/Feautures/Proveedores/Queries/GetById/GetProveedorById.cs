using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Feautures.Proveedores.Queries.GetById
{
    public class GetProveedorById: IRequest<Response<ProveedorDTO>>
    {
        public long Id { get; set; }
        public class GetProveedorByIdQueryHandler : IRequestHandler<GetProveedorById, Response<ProveedorDTO>>
        {
            private readonly IRepositoryAsync<Proveedor> _repository;
            private readonly IMapper _mapper;

            public GetProveedorByIdQueryHandler(IRepositoryAsync<Proveedor> repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            public async Task<Response<ProveedorDTO>> Handle(GetProveedorById request, CancellationToken cancellationToken)
            {
                var proveedor = await _repository.GetByIdAsync(request.Id);

                if (proveedor == null) throw new ApiException($"Proveedor no encontrado con el ID: {request.Id}");

                return new Response<ProveedorDTO>(_mapper.Map<ProveedorDTO>(proveedor));
            }
        }
    }
}
