using Application.Login.ViewModel;
using AutoMapper;
using Domain.Login.Entities;

namespace Application.Mapping
{
    public class ViewModelToDomainMappingProfile: Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<LoginEntryViewModel, LoginEntry>();
        }
    }
}
