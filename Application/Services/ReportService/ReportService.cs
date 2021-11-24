namespace Application.Services.ReportService
{
    using Application.DTOs.ReportDTOs;
    using Application.Extensions;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportService : BaseService, IReportService
    {
        public ReportService(MoneyManagerContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<AmountByMonthByCategoriesDTO>> GetAmountByMonthByCategories(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = new List<AmountByMonthByCategoriesDTO>();
                var userTransactions = await context.Transactions
                    .Where(x => x.UserId == userId && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                    .Include(x => x.TransactionCategory)
                    .ToListAsync();
                var userTransactionsByMonth = userTransactions.GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month });
                var userCategories = await context.TransactionCategories
                     .Where(x => x.UserId == userId && x.Description != "Universal")
                     .OrderBy(x => x.Description)
                     .ToListAsync();

                foreach(var userMonthTransaction in userTransactionsByMonth)
                {
                    var monthValues = new List<float>();
                    foreach (var category in userCategories)
                    {
                        var groupByCategories = userMonthTransaction
                            .Where(x => x.TransactionCategoryId == category.TransactionCategoryId);

                        monthValues.Add(groupByCategories.Sum(x => x.Amount.ConvertToCurrency("EUR", x.TransactionDate, context)));
                    }

                    result.Add(new AmountByMonthByCategoriesDTO
                    {
                        Data = monthValues,
                        Label = new DateTime(userMonthTransaction.Key.Year, userMonthTransaction.Key.Month, 1)
                                .ToString("Y"),
                        BorderWidth = 1
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<AmountByMonthByCategoriesDTO>> GetExpensesVsRefill(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var userTransaction = await context.Transactions
                    .Where(x => x.UserId == userId && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                    .ToListAsync();

                var refillId = (await context.TransactionTypes
                    .FirstOrDefaultAsync(x => x.Description == "Refill")).TransactionTypeId;

                var expensesId = (await context.TransactionTypes
                    .FirstOrDefaultAsync(x => x.Description == "Expenses")).TransactionTypeId;

                var monthPeriod = MonthDifference(startDate, endDate);

                var refills = new List<float>();
                var expenses = new List<float>();

                for (int i = 0; i < monthPeriod; i++)
                {
                    refills.Add(userTransaction
                        .Where(x => x.TransactionDate.Month == startDate.AddMonths(i).Month && x.TransactionDate.Year == startDate.AddMonths(i).Year && x.TransactionTypeId == refillId)
                        .Sum(x => x.Amount.ConvertToCurrency("EUR", x.TransactionDate, context)));

                    expenses.Add(userTransaction
                        .Where(x => x.TransactionDate.Month == startDate.AddMonths(i).Month && x.TransactionDate.Year == startDate.AddMonths(i).Year && x.TransactionTypeId == expensesId)
                        .Sum(x => x.Amount.ConvertToCurrency("EUR", x.TransactionDate, context)));
                }

                var result = new List<AmountByMonthByCategoriesDTO>();
                result.Add(new AmountByMonthByCategoriesDTO
                {
                    Data = refills,
                    Label = "Refill",
                    BorderWidth = 1
                });
                result.Add(new AmountByMonthByCategoriesDTO
                {
                    Data = expenses,
                    Label = "Expenses",
                    BorderWidth = 1
                });

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> GetMonthFromPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = new List<string>();
                var monthDif = MonthDifference(startDate, endDate);
                for (int i = 0; i < monthDif; i++) 
                {
                    result.Add(startDate.AddMonths(i).ToString("Y"));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private int MonthDifference(DateTime startDate, DateTime endDate)
        {
            return endDate.Year * 12 + endDate.Month - (startDate.Year * 12 + startDate.Month) + 1;
        }
    }
}
