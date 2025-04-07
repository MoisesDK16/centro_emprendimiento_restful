using Application.DTOs.Users;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.UsuarioC.Commands
{
    public class ActualizarUsuario  : IRequest<Response<string>>
    {
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? SegundoApellido { get; set; }
        public required string CiudadOrigen { get; set; }
        public string? Telefono { get; set; }
    }

    public class ActualizarUsuarioHandler : IRequestHandler<ActualizarUsuario, Response<string>>
    {
        private readonly IUserService _userService;
        public ActualizarUsuarioHandler(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<Response<string>> Handle(ActualizarUsuario request, CancellationToken cancellationToken)
        {
            if(_userService.UserExistsAsync(request.Id) == null)
                throw new ApiException("El usuario no existe");

            var result = await _userService.ActualizarUsuario(new UserInfo
            {
                Id = request.Id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                CiudadOrigen = request.CiudadOrigen,
                Telefono = request.Telefono,
                UserName = request.UserName,
                Email = request.Email,

            });

            if (result) return new Response<string>("Usuario actualizado correctamente");
            else throw new ApiException("Error al actualizar el usuario");
        }
    }
}
