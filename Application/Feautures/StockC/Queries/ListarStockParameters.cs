using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.StockC.Queries
{
    public class ListarStockParameters : RequestParameter
    {
        public long? ProductoId { get; set; }
        public required long NegocioId { get; set; }
        public int? Cantidad { get; set; }
        public DateOnly? FechaCaducidad { get; set; }
    }
}
