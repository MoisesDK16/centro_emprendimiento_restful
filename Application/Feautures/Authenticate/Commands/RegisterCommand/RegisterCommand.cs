﻿using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<Response<string>>
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

        //Negocio
        public required string NombreNegocio { get; set; }
        public string? Descripcion { get; set; }
        public required string Direccion { get; set; }
        public required string TelefonoNegocio { get; set; }

        public long CategoriaNegocio { get; set; }

    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<string>>
    {
        public readonly IAccountService _accountService;

        public RegisterCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterAsync(new RegistrarEmprendedor
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                Identificacion = request.Identificacion,
                Telefono = request.Telefono,
                CiudadOrigen = request.CiudadOrigen,
                NombreNegocio = request.NombreNegocio,
                Descripcion = request.Descripcion,
                DireccionNegocio = request.Direccion,
                TelefonoNegocio = request.TelefonoNegocio,
                CategoriaNegocio = request.CategoriaNegocio

            }, request.Origin);
        }
    }
}
