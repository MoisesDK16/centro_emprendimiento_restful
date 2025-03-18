using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.Authenticate.Commands.RegisterSellerCommand
{
    public class RegisterSellerCommand : IRequest<Response<string>>
    {
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string Identificacion { get; set; }
        public required string Telefono { get; set; }
        public required string CiudadOrigen { get; set; }
        public required string Origin { get; set; }

    }

    public class RegisterSellerHandler : IRequestHandler<RegisterSellerCommand, Response<string>>
    {
        public readonly IAccountService _accountService;

        public RegisterSellerHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<string>> Handle(RegisterSellerCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterVendedorAsync(new RegisterRequest
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Identificacion = request.Identificacion,
                Telefono = request.Telefono,
                CiudadOrigen = request.CiudadOrigen

            }, request.Origin);
        }
    }
}
