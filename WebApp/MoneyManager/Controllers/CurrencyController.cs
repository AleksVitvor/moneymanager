using Application.Services.CurrencyService;

namespace MoneyManager.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
    public class CurrencyController : BaseApiController
    {
        private readonly ICurrencyService currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyList()
        {
            try
            {
                return Ok(await currencyService.GetCurrencyList());
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
