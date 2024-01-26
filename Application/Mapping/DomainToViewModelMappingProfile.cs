using Application.Login.ViewModel;
using AutoMapper;
using Domain.Login.Entities;

namespace Application.Mapping
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<LoginEntry, LoginEntryViewModel>();
        }
    }
}
