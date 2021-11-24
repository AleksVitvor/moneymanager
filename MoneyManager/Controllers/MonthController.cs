namespace MoneyManager.Controllers
{
    using Application.Services.ReportService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    public class MonthController : BaseApiController
    {
        private readonly IReportService reportService;
        public MonthController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        public IActionResult GetMonthPeriod()
        {
            try
            {
                return Ok(reportService.GetMonthFromPeriod(DateTime.Now.AddYears(-1), DateTime.Now));
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
