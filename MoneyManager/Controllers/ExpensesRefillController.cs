namespace MoneyManager.Controllers
{
    using Application.Filters;
    using Application.Services.ReportService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize]
    public class ExpensesRefillController : BaseApiController
    {
        private readonly IReportService reportService;
        public ExpensesRefillController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpensesVsRefillDataAsync()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await reportService.GetExpensesVsRefill(id, DateTime.Today.AddYears(-1), DateTime.Today));
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
                    Message = "Error occurred while search for moth period"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetExpensesVsRefillReport(CategoryReportRequestFilter filter)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await reportService.GetExpensesVsRefill(id, filter.StartDate, filter.EndDate));
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
                    Message = "Error occurred while search for moth period"
                });
            }
        }
    }
}
