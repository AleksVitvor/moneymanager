namespace MoneyManager.Controllers
{
    using Application.Services.LoginService;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    public class RoleController : BaseApiController
    {
        private readonly IUserService userService;
        public RoleController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPut]
        public async Task<IActionResult> ChangeUserActive([FromQuery]int id)
        {
            try
            {
                return Ok(await userService.ChangeUserRole(id));
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
