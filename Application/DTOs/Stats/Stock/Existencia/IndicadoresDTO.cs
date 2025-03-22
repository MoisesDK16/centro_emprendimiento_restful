using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stats.Stock.Existencia
{
    public class IndicadoresDTO
    {
        public int ExistenciaInicial { get; set; }

        public decimal CostoInicial { get; set; }

        public int CantidadEntrada { get; set; }

        public decimal CostoEntrada { get; set; }

        public int CantidadSalida { get; set; }

        public decimal IngresosSalida { get; set; }

        public int CantidadExistente { get; set; }

        public decimal CostoExistente { get; set; }

        public int CantidadFinal { get; set; }

        public decimal CostoFinal { get; set; }

    }
}
