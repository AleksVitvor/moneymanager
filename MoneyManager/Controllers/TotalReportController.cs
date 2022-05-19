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
    public class TotalReportController : BaseApiController
    {
        private readonly IReportService reportService;
        public TotalReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalReportDataAsync()
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await reportService.GetTotalReport(id, new TotalReportFilter
                    {
                        CurrencyId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                        EndDate = DateTime.Now
                    }));
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
        public async Task<IActionResult> GetExpensesVsRefillReport(TotalReportFilter filter)
        {
            try
            {
                _ = int.TryParse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value, out int id);

                if (id > 0)
                {
                    return Ok(await reportService.GetTotalReport(id, filter));
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
