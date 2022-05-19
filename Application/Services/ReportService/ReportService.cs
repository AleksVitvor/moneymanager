namespace Application.Services.ReportService
{
    using DTOs.ReportDTOs;
    using Extensions;
    using Filters;
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
                    .Include(x => x.Currency)
                    .Where(x => x.UserId == userId && x.TransactionDate >= startDate && x.TransactionDate <= endDate && x.TransactionTypeId == 1)
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

                        monthValues.Add(groupByCategories.Sum(x => x.Amount.ConvertToCurrency("EUR", x.Currency.CurrencyCode, x.TransactionDate, context)));
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

        public async Task<List<AmountByMonthByCategoriesDTO>> GetCategoryReport(int userId, CategoryReportRequestFilter filter)
        {
            try
            {
                var result = new List<AmountByMonthByCategoriesDTO>();
                var userTransactions = await context.Transactions
                    .Include(x => x.Currency)
                    .Include(x => x.TransactionCategory)
                    .Where(x => x.UserId == userId && x.TransactionDate >= filter.StartDate && x.TransactionDate <= filter.EndDate && filter.CategoriesList.Contains(x.TransactionCategoryId) && x.TransactionTypeId == filter.TransactionTypeId)
                    .ToListAsync();

                var userTransactionsByMonth = userTransactions.GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month });

                var userCategories = await context.TransactionCategories
                     .Where(x => x.UserId == userId && x.Description != "Universal" && filter.CategoriesList.Contains(x.TransactionCategoryId))
                     .OrderBy(x => x.Description)
                     .ToListAsync();

                var currency = await context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == filter.CurrencyId);

                foreach (var userMonthTransaction in userTransactionsByMonth)
                {
                    var monthValues = new List<float>();
                    foreach (var category in userCategories)
                    {
                        var groupByCategories = userMonthTransaction
                            .Where(x => x.TransactionCategoryId == category.TransactionCategoryId);

                        monthValues.Add(groupByCategories.Sum(x => x.Amount.ConvertToCurrency(currency.CurrencyCode, x.Currency.CurrencyCode, x.TransactionDate, context)));
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
                    .Include(x => x.Currency)
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
                        .Sum(x => x.Amount.ConvertToCurrency("EUR", x.Currency.CurrencyCode, x.TransactionDate, context)));

                    expenses.Add(userTransaction
                        .Where(x => x.TransactionDate.Month == startDate.AddMonths(i).Month && x.TransactionDate.Year == startDate.AddMonths(i).Year && x.TransactionTypeId == expensesId)
                        .Sum(x => x.Amount.ConvertToCurrency("EUR", x.Currency.CurrencyCode, x.TransactionDate, context)));
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

        public async Task<List<AmountByMonthByCategoriesDTO>> GetExpensesVsRefill(int userId, DateTime startDate, DateTime endDate, int currencyId)
        {
            try
            {
                var userTransaction = await context.Transactions
                    .Include(x => x.Currency)
                    .Where(x => x.UserId == userId && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                    .ToListAsync();

                var refillId = (await context.TransactionTypes
                    .FirstOrDefaultAsync(x => x.Description == "Refill")).TransactionTypeId;

                var expensesId = (await context.TransactionTypes
                    .FirstOrDefaultAsync(x => x.Description == "Expenses")).TransactionTypeId;

                var monthPeriod = MonthDifference(startDate, endDate);

                var refills = new List<float>();
                var expenses = new List<float>();

                var currency = await context.Currencies.FirstOrDefaultAsync(x => x.CurrencyId == currencyId);

                for (int i = 0; i < monthPeriod; i++)
                {
                    refills.Add(userTransaction
                        .Where(x => x.TransactionDate.Month == startDate.AddMonths(i).Month && x.TransactionDate.Year == startDate.AddMonths(i).Year && x.TransactionTypeId == refillId)
                        .Sum(x => x.Amount.ConvertToCurrency(currency.CurrencyCode, x.Currency.CurrencyCode, x.TransactionDate, context)));

                    expenses.Add(userTransaction
                        .Where(x => x.TransactionDate.Month == startDate.AddMonths(i).Month && x.TransactionDate.Year == startDate.AddMonths(i).Year && x.TransactionTypeId == expensesId)
                        .Sum(x => x.Amount.ConvertToCurrency(currency.CurrencyCode, x.Currency.CurrencyCode, x.TransactionDate, context)));
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

        public async Task<List<TotalReportDTO>> GetTotalReport(int userId, TotalReportFilter filter)
        {
            try
            {
                var currency = await context.Currencies.FirstOrDefaultAsync(x=>x.CurrencyId == filter.CurrencyId);
                var result = new List<TotalReportDTO>();
                var startDate = new DateTime(filter.StartDate.Year, filter.StartDate.Month, 1);
                var endDate = new DateTime(filter.EndDate.Year, filter.EndDate.Month, DateTime.DaysInMonth(filter.EndDate.Year, filter.EndDate.Month));

                var transactions = await context.Transactions
                    .Include(x => x.Currency)
                    .Include(x => x.TransactionType)
                    .Include(x => x.TransactionCategory)
                    .Where(x => x.UserId == userId && x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                    .ToListAsync();

                var groupedTransaction = transactions
                    .GroupBy(x => new { x.TransactionDate.Year, x.TransactionDate.Month })
                    .ToDictionary(x => x.Key, y => y.ToList());

                foreach(var monthTransactionGroup in groupedTransaction)
                {
                    var monthTransaction = new List<TotalReportTransactionDTO>();

                    foreach(var transaction in monthTransactionGroup.Value)
                    {
                        var transactionDTO = new TotalReportTransactionDTO
                        {
                            Amount = transaction.Amount.ConvertToCurrency(currency.CurrencyCode, transaction.Currency.CurrencyCode, transaction.TransactionDate, context),
                            Category = transaction.TransactionCategory.Description,
                            Type = transaction.TransactionType.Description,
                            Date = transaction.TransactionDate.ToString("d"),
                            Currency = currency.CurrencyCode
                        };

                        monthTransaction.Add(transactionDTO);
                    }

                    var refill = monthTransaction.Where(x => x.Type == "Refill").Sum(x => x.Amount);
                    var expenses = monthTransaction.Where(x => x.Type == "Expenses").Sum(x => x.Amount);

                    var total = new TotalReportDTO
                    {
                        Currency = currency.CurrencyCode,
                        Refill = refill,
                        Expenses = expenses,
                        Month = new DateTime(monthTransactionGroup.Key.Year, monthTransactionGroup.Key.Month, 1).ToString("MMM/yyy"),
                        MonthTransactions = monthTransaction,
                        Date = new DateTime(monthTransactionGroup.Key.Year, monthTransactionGroup.Key.Month, 1),
                        Total = refill - expenses
                    };

                    result.Add(total);
                }

                return result.OrderByDescending(x => x.Date).ToList();
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
