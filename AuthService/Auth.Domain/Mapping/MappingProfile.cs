using AutoMapper;
using Auth.Domain.Models;
using Auth.Domain.Entites;

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
                .ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<StudentEntity, Student>().ReverseMap();
        }
    }
}
