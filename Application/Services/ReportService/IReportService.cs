namespace Application.Services.ReportService
{
    using Application.DTOs.ReportDTOs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReportService
    {
        Task<List<AmountByMonthByCategoriesDTO>> GetAmountByMonthByCategories(int userId, DateTime startDate, DateTime endDate);

        Task<List<AmountByMonthByCategoriesDTO>> GetExpensesVsRefill(int userId, DateTime startDate, DateTime endDate);

        List<string> GetMonthFromPeriod(DateTime startDate, DateTime endDate);
    }
}
