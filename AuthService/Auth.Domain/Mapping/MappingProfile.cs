using Auth.Domain.Entites;
using Auth.Domain.Enums;
using Auth.Domain.Models;
using AutoMapper;

namespace Auth.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserEntity>()
                 .ForMember(dest => dest.Roles, opt => opt.Ignore())
                 .ForMember(dest => dest.Student, opt => opt.Ignore())
                 .ForMember(dest => dest.Teacher, opt => opt.Ignore())
                 .ForMember(dest => dest.Admin, opt => opt.Ignore());

            CreateMap<UserEntity, User>()
                 .ForMember(dest => dest.Roles,
                            opt => opt.MapFrom(src => src.Roles.Select(r => (Role)r.Id).ToList()));

            CreateMap<StudentEntity, Student>().ReverseMap();
        }
    }
}
