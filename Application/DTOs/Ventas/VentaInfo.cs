using Application.DTOs.Detalles;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Ventas
{
    public class VentaInfo : GeneralVenta
    {
        public List<DetalleInfo> Detalles { get; set; } = new List<DetalleInfo>();
    }
}
