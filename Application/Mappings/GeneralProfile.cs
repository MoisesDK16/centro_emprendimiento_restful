using Application.DTOs;
using Application.DTOs.Promociones;
using Application.Feautures.CategoriaC.Commands;
using Application.Feautures.ProductoC.Commands;
using Application.Feautures.Proveedores.Commands.CreateProveedorCommand;
using Application.Feautures.StockC.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile: Profile
    {

        public GeneralProfile() {

            CreateMap<Proveedor, ProveedorDTO>();
            CreateMap<CreateProveedorCommand, Proveedor>();
            CreateMap<CrearCategoriaComando, Categoria>();
            // Mapeo de CrearStock a Stock, estableciendo manualmente ProductoId y Producto
            CreateMap<CrearStock, Stock>()
                .ForMember(dest => dest.Producto, opt => opt.Ignore()); // Ignoramos la navegación aquí
        }
    }
}
