using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Correos
{
    public class EnvioInformacionDTO
    {
        public required CorreoDTO EnvioInformacion { get; set; }
        public required List<string> Correos { get; set; }
    }

}
