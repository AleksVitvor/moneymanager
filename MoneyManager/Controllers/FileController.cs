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
                    var folderName = Path.Combine("Resources", "Images");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    
                    if (file.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        await transactionService.AddPhotoTransaction(id, dbPath);

                        System.IO.File.Delete(fullPath);
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
