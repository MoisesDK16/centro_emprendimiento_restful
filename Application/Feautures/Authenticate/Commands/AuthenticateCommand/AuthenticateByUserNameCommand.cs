using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Feautures.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticateByUserNameCommand : IRequest<Response<AuthenticationResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }

    public class AuthenticateByUserNameHandler : IRequestHandler<AuthenticateByUserNameCommand, Response<AuthenticationResponse>>
    {
        public readonly IAccountService _accountService;

        public AuthenticateByUserNameHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<Response<AuthenticationResponse>> Handle(AuthenticateByUserNameCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.logInByUserName(new AuthenticationRequestUserName
            {
                UserName = request.UserName,
                Password = request.Password,
            },
            request.IpAddress);
        }
    }
}
