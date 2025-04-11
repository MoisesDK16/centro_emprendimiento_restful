using Application.DTOs.Users;
using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.UsuarioC.Queries
{
    public class ListarVendedores : IRequest<PagedResponse<List<UserVendedor>>>
    {
        public long NegocioId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Identificacion { get; set; }
    }

    public class ListarVendedoresHandler : IRequestHandler<ListarVendedores, PagedResponse<List<UserVendedor>>>
    {
        private readonly IUserService _userService;
        public ListarVendedoresHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<PagedResponse<List<UserVendedor>>> Handle(ListarVendedores request, CancellationToken cancellationToken)
        {
            var vendedores = await _userService.ListarVendedores();

            if(request.NegocioId > 0) vendedores = vendedores.Where(x => x.NegociosInfo.Any(n => n.Id == request.NegocioId)).ToList();

            if (!string.IsNullOrEmpty(request.Email)) vendedores = vendedores.Where(x => x.Email.Contains(request.Email, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(request.UserName)) vendedores = vendedores.Where(x => x.UserName.Contains(request.UserName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!string.IsNullOrEmpty(request.Identificacion)) vendedores = vendedores.Where(x => x.Identificacion.Contains(request.Identificacion, StringComparison.OrdinalIgnoreCase)).ToList();

            var totalCount = vendedores.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
            var pagedVendedores = vendedores
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<List<UserVendedor>>(pagedVendedores, request.PageNumber, request.PageSize, totalPages, totalCount);
        }
    }

    public class ListarVendedoresParameters : RequestParameter
    {
        public long NegocioId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Identificacion { get; set; }

    }
}
