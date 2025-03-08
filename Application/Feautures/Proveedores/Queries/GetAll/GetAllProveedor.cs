using Application.DTOs;
using Application.Interfaces;
using Application.Specifications;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Application.Feautures.Proveedores.Queries.GetAll
{
    public class GetAllProveedor: IRequest<PagedResponse<IEnumerable<ProveedorDTO>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string nombre { get; set; }
        public string ruc { get; set; }

        public class GetAllClientesQueryHandler : IRequestHandler<GetAllProveedor, PagedResponse<IEnumerable<ProveedorDTO>>>
        {
            private readonly IReadOnlyRepositoryAsync<Proveedor> _repository;
            private readonly IDistributedCache _distributedCache;
            private readonly IMapper _mapper;
            public GetAllClientesQueryHandler(IReadOnlyRepositoryAsync<Proveedor> repository, IMapper mapper, IDistributedCache distributedCache)
            {
                _repository = repository;
                _mapper = mapper;
                _distributedCache = distributedCache;
            }
            public async Task<PagedResponse<IEnumerable<ProveedorDTO>>> Handle(GetAllProveedor request, CancellationToken cancellationToken)
            {
                var cacheKey = $"proveedorsList_{request.PageSize}_{request.PageNumber}_{request.nombre}_{request.ruc}";
                var proveedores = new List<Proveedor>();
                var redisproveedorsList = await _distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(redisproveedorsList))
                {
                    proveedores = JsonConvert.DeserializeObject<List<Proveedor>>(redisproveedorsList);
                }
                else
                {
                    proveedores = await _repository.ListAsync(
                        new ProveedorSpecification(request.PageSize, request.PageNumber, request.nombre, request.ruc)
                    );

                    var serializedproveedorsList = JsonConvert.SerializeObject(proveedores);
                    var encodedData = Encoding.UTF8.GetBytes(serializedproveedorsList);

                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.UtcNow.AddMinutes(10)) 
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                    await _distributedCache.SetStringAsync(cacheKey, serializedproveedorsList, options).ConfigureAwait(false);
                }

                var proveedoresDTO = _mapper.Map<IEnumerable<ProveedorDTO>>(proveedores);

                return new PagedResponse<IEnumerable<ProveedorDTO>>(proveedoresDTO, request.PageNumber, request.PageSize);
            }

        }

    }
}
