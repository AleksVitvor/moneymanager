namespace MoneyManager.Controllers
{
    using Application.DTOs.TransactionDTOs;
    using Application.Services.TransactionService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class TransactionController : BaseApiController
    {
        private readonly ITransactionService transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await transactionService.GetTransactions(id));
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
                    Message = "Error occurred while search for transactions"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(IncommingTransactionDTO transactionDTO)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    transactionDTO.UserId = id;
                    return Ok(await transactionService.CreateTransaction(transactionDTO));
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
                    Message = "Error occurred while create transaction"
                });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(IncommingTransactionDTO item)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    item.UserId = id;
                    return Ok(await transactionService.UpdateTransaction(item));
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
                    Message = "Error occurred while create transaction"
                });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value,
                    out int userId);

                if (userId > 0)
                {
                    return Ok(await transactionService.DeleteTransaction(id, userId));
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
                    Message = "Error occurred while create transaction"
                });
            }
        }
    }
}
