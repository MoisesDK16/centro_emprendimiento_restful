using Application.DTOs;
using Application.Feautures.CategoriaC.Commands;
using Application.Feautures.ProductoC.Commands;
using Application.Feautures.Proveedores.Commands.CreateProveedorCommand;
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
            CreateMap<CrearProducto, Producto>();
            CreateMap<ActualizarProducto, Producto>();
        }
    }
}
