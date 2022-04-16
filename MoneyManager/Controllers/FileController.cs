namespace MoneyManager.Controllers
{
    using Application.Services;
    using Application.Services.LoginService;
    using Application.Services.TransactionService;
    using Azure.Storage.Blobs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class FileController: BaseApiController
    {
        private readonly IUserService userService;
        private readonly ITransactionService transactionService;

        public FileController(IUserService userService, ITransactionService transactionService)
        {
            this.userService = userService;
            this.transactionService = transactionService;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    var user = await userService.GetUserById(id);

                    var formCollection = await Request.ReadFormAsync();
                    var file = formCollection.Files.First();
                    
                    if (file.Length > 0)
                    {
                        await transactionService.AddPhotoTransaction(file, id);

                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
