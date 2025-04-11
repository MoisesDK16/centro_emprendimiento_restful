using Application.DTOs.Users;
using Application.Interfaces;
using Application.Parameters;
using Application.Wrappers;
using Domain.Enums.Negocio;
using MediatR;

namespace Application.Feautures.UsuarioC.Queries
{
    public class ListarEmprendedores : IRequest<PagedResponse<List<UserEmprendedor>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Identificacion { get; set; }
    }

    public class ListarEmprendedoresHandler : IRequestHandler<ListarEmprendedores, PagedResponse<List<UserEmprendedor>>>
    {
        private readonly IUserService _userService;
        public ListarEmprendedoresHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<PagedResponse<List<UserEmprendedor>>> Handle(ListarEmprendedores request, CancellationToken cancellationToken)
        {
            var usuarios = await _userService.ListarEmprendedores();

            // Aplicar filtros dinámicos
            if (!string.IsNullOrEmpty(request.Email))
            {
                usuarios = usuarios.Where(u => u.Email.Contains(request.Email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(request.UserName))
            {
                usuarios = usuarios.Where(u => u.UserName.Contains(request.UserName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(request.Identificacion))
            {
                usuarios = usuarios.Where(u => u.Identificacion.Contains(request.Identificacion, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var TotalRecords = usuarios.Count();
            var TotalPages = (int)Math.Ceiling((double)TotalRecords / request.PageSize);

            // Paginación
            usuarios = usuarios
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return new PagedResponse<List<UserEmprendedor>>(usuarios, request.PageNumber, request.PageSize, TotalPages, TotalRecords);
        }

    }

    public class ListarEmprendedoresParameters: RequestParameter
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Identificacion { get; set; }
    }
}
