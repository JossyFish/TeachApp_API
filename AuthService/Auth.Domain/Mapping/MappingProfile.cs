using AutoMapper;
using Auth.Domain.Models;
using Auth.Domain.Entites;

namespace Auth.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
