using Application.Login.ViewModel;
using Application.Usuario.ViewModel;
using AutoMapper;
using Domain.Login.Entities;
using Domain.Usuario.Entidades;

namespace Application.Mapping
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<LoginEntryViewModel, LoginEntry>();
            CreateMap<UsuariosViewModel, Usuarios>()            
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password));
        }
    }
}
