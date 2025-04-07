using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.UsuarioC.Queries
{
    public class EmprendedorById : IRequest<Response<UserEmprendedor>>
    {
        public required string Id { get; set; }
    }

    public class EmprendedorByIdHandler : IRequestHandler<EmprendedorById, Response<UserEmprendedor>>
    {
        private readonly IUserService _usuarioService;
        public EmprendedorByIdHandler(IUserService usuarioService)
        {
            _usuarioService = usuarioService;
        }
        public async Task<Response<UserEmprendedor>> Handle(EmprendedorById request, CancellationToken cancellationToken)
        {
            var emprendedor = await _usuarioService.GetEmprendedorInfoAsync(request.Id);

            return new Response<UserEmprendedor>(emprendedor);
        }
    }
}
