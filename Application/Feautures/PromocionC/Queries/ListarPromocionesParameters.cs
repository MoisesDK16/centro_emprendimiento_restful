using Application.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feautures.PromocionC.Queries
{
    public class ListarPromocionesParameters : RequestParameter
    {
        public long NegocioId { get; set; }
    }
}
