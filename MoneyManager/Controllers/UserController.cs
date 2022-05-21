namespace MoneyManager.Controllers
{
    using Application.Services.LoginService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserList()
        {
            try
            {
                return Ok(await userService.GetUserList());
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "Error occurred while search for users"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserActive([FromQuery]int id)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int adminId);

                return Ok(await userService.ChangeUserActive(id, adminId));
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Message = "Error occurred while search for users"
                });
            }
        }
    }
}
