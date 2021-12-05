namespace MoneyManager.Controllers
{
    using Application.DTOs.TransactionDTOs;
    using Application.Filters;
    using Application.Services.TransactionCategoriesService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class UserCategoryController : BaseApiController
    {
        private readonly ITransactionCategoriesService transactionService;
        public UserCategoryController(ITransactionCategoriesService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> GetTransactionCategories(CategoryReportRequestFilter filter)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await transactionService.GetTransactionCategories(id, filter.CategoriesList));
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
    }
}
