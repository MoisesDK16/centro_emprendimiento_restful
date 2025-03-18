using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class AuthenticationRequestUserName
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
