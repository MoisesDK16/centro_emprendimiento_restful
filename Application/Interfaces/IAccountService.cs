using Application.DTOs.Users;
using Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> logInByEmail(AuthenticationRequestEmail request, string ipAdress);

        Task<Response<AuthenticationResponse>> logInByUserName(AuthenticationRequestUserName request, string ipAdress);

        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);

        Task<Response<string>> RegisterVendedorAsync(RegisterRequest request, string origin);
    }
}
