using Application.DTOs.UserDTOs;
using Domain;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Application.Services.LoginService
{
    public interface IUserService
    {
        Task<LoginDTO> GetUserByEmail(string email, string password);

        Task<LoginDTO> GetUserById(int id);

        Task RegisterUser(RegistrationModelDTO registrationModel);

        Task<List<ManageUserDTO>> ChangeUserActive(int id, int userModifierId);

        Task<List<ManageUserDTO>> ChangeUserRole(int id);

        Task<List<ManageUserDTO>> GetUserList();
    }
}
