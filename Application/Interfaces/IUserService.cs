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
        Task<UserInfo> GetUserInfoAsync(string userId);
    }
}
