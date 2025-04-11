using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class ResetPassword
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Token { get; set; }
    }
}
