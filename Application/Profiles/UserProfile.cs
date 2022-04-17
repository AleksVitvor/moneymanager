namespace Application.Profiles
{
    using Application.DTOs.UserDTOs;
    using Application.Services;
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

            CreateMap<RegistrationModelDTO, User>()
                .ForMember(res => res.FullName, src => src.MapFrom(x => x.FirstName + " " + x.LastName))
                .ForMember(res => res.Email, src => src.MapFrom(x => x.Email))
                .ForMember(res => res.Password, src => src.MapFrom(x => CryptoService.ComputeHash(x.Password)))
                .ForMember(res => res.Username, src => src.MapFrom(x => x.Username))
                .ForMember(res => res.IsActive, src => src.MapFrom(x => true))
                .ForMember(res => res.RoleId, src => src.MapFrom(x => 1));

            CreateMap<User, ManageUserDTO>()
                .ForMember(res => res.Id, src => src.MapFrom(x => x.UserId))
                .ForMember(res => res.FullName, src => src.MapFrom(x => x.FullName))
                .ForMember(res => res.Role, src => src.MapFrom(x => x.Role.Description))
                .ForMember(res => res.Email, src => src.MapFrom(x => x.Email))
                .ForMember(res => res.IsActive, src => src.MapFrom(x => x.IsActive));
        }
    }
}
