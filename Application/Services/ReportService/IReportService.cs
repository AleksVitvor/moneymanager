namespace Application.Services.ReportService
{
    using Application.DTOs.ReportDTOs;
    using Application.Filters;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IReportService
    {
        Task<List<AmountByMonthByCategoriesDTO>> GetAmountByMonthByCategories(int userId, DateTime startDate, DateTime endDate);

        Task<List<AmountByMonthByCategoriesDTO>> GetExpensesVsRefill(int userId, DateTime startDate, DateTime endDate);

        Task<List<AmountByMonthByCategoriesDTO>> GetExpensesVsRefill(int userId, DateTime startDate, DateTime endDate, int currencyId);

        List<string> GetMonthFromPeriod(DateTime startDate, DateTime endDate);

        Task<List<AmountByMonthByCategoriesDTO>> GetCategoryReport(int userId, CategoryReportRequestFilter filter);
    }
}
