using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires; // Has the token expired?

        public DateTime Created { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime Revoked { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplaceByToken { get; set; }

        public bool isActive => Revoked == null && !IsExpired;



    }
}
