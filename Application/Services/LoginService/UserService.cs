using System.Linq;
using Application.Exceptions;

namespace Application.Services.LoginService
{
    using DTOs.UserDTOs;
    using AutoMapper;
    using Domain;
    using Infrastructure.DefaultSettings;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Threading.Tasks;

    public class UserService : BaseService, IUserService 
    {
        public UserService(MoneyManagerContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<LoginDTO> GetUserByEmail(string email, string password)
        {
            try
            {
                var user = mapper.Map<LoginDTO>(await context.Users
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Email == email && x.Password == password));

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LoginDTO> GetUserById(int id)
        {
            try
            {
                var user = mapper.Map<LoginDTO>(await context.Users
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.UserId == id));

                return user;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RegisterUser(RegistrationModelDTO registrationModel)
        {
            try
            {
                var user = mapper.Map<User>(registrationModel);
                user.TransactionCategories = DefaultTransactionCategories.TransactionCategories;
                if (context.Users.Any(x => x.Email == user.Email))
                {
                    throw new UserValidationException("User with the same email already exists");
                }
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
