using Application.Exceptions;

namespace MoneyManager.Controllers
{
    using Application.DTOs.UserDTOs;
    using Application.Services.LoginService;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class RegisterController : BaseApiController
    {
        private readonly IUserService userService;

        public RegisterController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegistrationModelDTO registrationModel)
        {
            try
            {
                await userService.RegisterUser(registrationModel);
                return Ok(new
                {
                    Message = "Registration finished successfully"
                });
            }
            catch (UserValidationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occurred during registration");
            }
        }
    }
}
