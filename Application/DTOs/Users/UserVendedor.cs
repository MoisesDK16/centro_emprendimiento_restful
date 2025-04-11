using Application.DTOs.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Users
{
    public class UserVendedor : UserInfo
    {
        public List<NegocioInfoDTO> NegociosInfo { get; set; } = new List<NegocioInfoDTO>();
    }
}
