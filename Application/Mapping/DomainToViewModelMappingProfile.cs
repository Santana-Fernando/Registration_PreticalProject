using Application.Login.ViewModel;
using Application.Usuario.ViewModel;
using AutoMapper;
using Domain.Login.Entities;
using Domain.Usuario.Entidades;

namespace Application.Mapping
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<LoginEntry, LoginEntryViewModel>();
            CreateMap<Usuarios, UsuariosViewModel>()
            .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.email))
            .ForMember(dest => dest.password, opt => opt.MapFrom(src => src.password));
        }
    }
}
