namespace MoneyManager.Controllers
{
    using Application.DTOs.TransactionDTOs;
    using Application.Services.TransactionCategoriesService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class TransactionCategoriesController : BaseApiController
    {
        private readonly ITransactionCategoriesService transactionService;
        public TransactionCategoriesController(ITransactionCategoriesService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionCategories()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await transactionService.GetTransactionCategories(id));
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
                    Message = "Error occurred while search for transaction categories"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionCategory(NewTransactionCategoryDTO newCategory)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await transactionService.CreateCategory(id, newCategory.NewCategory));
                }

                return BadRequest(new
                {
                    Message = "User can't be found"
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Message = "Error occurred while create transaction category"
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await transactionService.RemoveCategory(id, categoryId));
                }

                return BadRequest(new
                {
                    Message = "User can't be found"
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    Message = "Error occurred while remove transaction category"
                });
            }
        }
    }
}
