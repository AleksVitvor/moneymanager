using Application.DTOs.UserDTOs;
using Application.Services;
using Application.Services.LoginService;
using Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly ILoginService loginService;
        public AuthController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(RequestLoginDTO requestLogin)
        {
            try
            {
                var user = await loginService.GetUserByEmail(
                    requestLogin.Username, 
                    CryptoService.ComputeHash(requestLogin.Password));

                var identity = await this.Authenticate(user);

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
                return StatusCode(500, "Authentication failed");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await loginService.GetUserById(id));
                }

                return BadRequest("User can't be found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Authentication failed");
            }
        }

        private async Task<ClaimsIdentity> Authenticate(LoginDTO loginModel)
        {
            if (loginModel != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, loginModel.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, loginModel.Role)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                return id;
            }

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
