namespace MoneyManager.Controllers
{
    using Application.DTOs.UserDTOs;
    using Application.Services;
    using Application.Services.LoginService;
    using Infrastructure.Options;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AuthController : BaseApiController
    {
        private readonly IUserService userService;
        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(RequestLoginDTO requestLogin)
        {
            try
            {
                var user = await userService.GetUserByEmail(
                    requestLogin.Username, 
                    CryptoService.ComputeHash(requestLogin.Password));

                var identity = this.Authenticate(user);

                if (user == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var encodedJwt = GetJwtToken(user, identity);

                var response = new
                {
                    token = encodedJwt,
                    user = user
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Authentication failed"
                });
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await userService.GetUserById(id));
                }

                return BadRequest(new
                {
                    Message = "User can't be found"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Authentication failed"
                });
            }
        }

        private ClaimsIdentity Authenticate(LoginDTO loginModel)
        {
            if (loginModel != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, loginModel.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, loginModel.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        private string GetJwtToken(LoginDTO login, ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
