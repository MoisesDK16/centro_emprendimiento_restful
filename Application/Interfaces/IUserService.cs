using Application.DTOs.Negocios;
using Application.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string userId);
        Task<UserEmprendedor> GetEmprendedorInfoAsync(string userId);
        Task<bool> Confirmar(string token, string userId);
        Task<bool> ActualizarUsuario(UserInfo userInfo);
        Task<List<UserEmprendedor>> ListarEmprendedores();

        Task<List<UserVendedor>> ListarVendedores();
        Task<bool> IsAdmin(string userId);
        Task<bool> IsEmprendedor(string userId);
        Task<bool> IsVendedor(string userId);
    }
}
