using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticateByEmailCommand: IRequest<Response<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }

    public class AuthenticateByEmailHandler : IRequestHandler<AuthenticateByEmailCommand, Response<AuthenticationResponse>>
    {
        public readonly IAccountService _accountService;

        public AuthenticateByEmailHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<AuthenticationResponse>> Handle(AuthenticateByEmailCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.logInByEmail(new AuthenticationRequestEmail 
            { 
                Email= request.Email,
                Password= request.Password,
            },
            request.IpAddress);
        }
    }
}
