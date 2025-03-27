using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.Min_Max
{
    public class CostoAlmacenadoPorMesDTO
    {
        public int Mes { get; set; }
        public decimal Costo { get; set; }
    }
}
