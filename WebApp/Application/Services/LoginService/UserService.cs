﻿namespace Application.Services.LoginService
{
    using Application.Exceptions;
    using AutoMapper;
    using Domain;
    using DTOs.UserDTOs;
    using Infrastructure.DefaultSettings;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : BaseService, IUserService 
    {
        public UserService(MoneyManagerContext context, IMapper mapper) : base(context, mapper)
        {

        }

        public async Task<List<ManageUserDTO>> ChangeUserActive(int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                user.IsActive = !user.IsActive;
                context.Users.Update(user);
                await context.SaveChangesAsync();

                return await GetUserList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ManageUserDTO>> ChangeUserRole(int id)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == id);
                if (user.RoleId == 1)
                {
                    user.RoleId = 2;
                }
                else
                {
                    user.RoleId = 1;
                }

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return await GetUserList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LoginDTO> GetUserByEmail(string email, string password)
        {
            try
            {
                var user = mapper.Map<LoginDTO>(await context.Users
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Email == email && x.Password == password && x.IsActive));

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

        public async Task<List<ManageUserDTO>> GetUserList()
        {
            try
            {
                var users = await context.Users
                    .Include(x => x.Role)
                    .ToListAsync();

                return mapper.Map<List<ManageUserDTO>>(users);
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
                user.TransactionCategories = new DefaultTransactionCategories().TransactionCategories;
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
