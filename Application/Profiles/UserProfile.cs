namespace Application.Profiles
{
    using Application.DTOs.UserDTOs;
    using AutoMapper;
    using Domain;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, LoginDTO>()
                .ForMember("DisplayName", src => src.MapFrom(x => x.FullName))
                .ForMember("Role", src => src.MapFrom(x => x.Role.Description))
                .ForMember("Id", src => src.MapFrom(x => x.UserId));
        }
    }
}
