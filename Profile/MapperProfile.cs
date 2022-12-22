using AutoMapper;
using tl2_tp4_2022_loboser.Models;
using tl2_tp4_2022_loboser.ViewModels;

namespace tl2_tp4_2022_loboser.Profiles{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cadete, CadeteViewModel>().ReverseMap();
            CreateMap<Cadete, AltaCadeteViewModel>().ReverseMap();
            CreateMap<Cadete, EditarCadeteViewModel>().ReverseMap();
            
            CreateMap<Pedido, PedidoViewModel>().ReverseMap();
            CreateMap<Pedido, AltaPedidoViewModel>().ReverseMap();
            CreateMap<Pedido, EditarPedidoViewModel>().ReverseMap();

            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            CreateMap<Cliente, EditarClienteViewModel>().ReverseMap();
            CreateMap<Cliente, AltaClienteViewModel>().ReverseMap();

            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
            CreateMap<Usuario, EditarUsuarioViewModel>().ReverseMap();
            CreateMap<Usuario, LogeoViewModel>().ReverseMap();
            CreateMap<Usuario, RegistroViewModel>().ReverseMap();
            CreateMap<Cliente, RegistroViewModel>().ReverseMap();
        }
    }
}

