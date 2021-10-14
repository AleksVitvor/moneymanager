using Application.DTOs.UserDTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Application.Services.LoginService
{
    public interface ILoginService
    {
        Task<LoginDTO> GetUserByEmail(string email, string password);

        Task<LoginDTO> GetUserById(int id);
    }
}
