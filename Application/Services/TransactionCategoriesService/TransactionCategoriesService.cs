using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.TransactionDTOs;
using AutoMapper;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services.TransactionCategoriesService
{
    public class TransactionCategoriesService: BaseService, ITransactionCategoriesService
    {
        public TransactionCategoriesService(MoneyManagerContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<List<TransactionCategoryDTO>> GetTransactionCategories(int userId)
        {
            try
            {
                var transactions = await context.TransactionCategories
                    .Where(x => x.UserId == userId && x.Description != "Universal")
                    .ToListAsync();
                return mapper.Map<List<TransactionCategory>, List<TransactionCategoryDTO>>(transactions);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionCategoryDTO>> CreateCategory(int userId, string newCategory)
        {
            try
            {
                await context.TransactionCategories.AddAsync(new TransactionCategory()
                {
                    Description = newCategory,
                    UserId = userId
                });
                await context.SaveChangesAsync();
                return await GetTransactionCategories(userId);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<List<TransactionCategoryDTO>> RemoveCategory(int userId, int categoryId)
        {
            try
            {
                var universalCategory =
                    await context.TransactionCategories.FirstOrDefaultAsync(x =>
                        x.Description == "Universal" && x.UserId == userId);

                var transactionWithRemoveCategory = await context.Transactions
                    .Where(x => x.TransactionCategoryId == categoryId && x.UserId == userId)
                    .ToListAsync();

                foreach (var transaction in transactionWithRemoveCategory)
                {
                    transaction.TransactionCategoryId = universalCategory.TransactionCategoryId;
                }

                context.Transactions.UpdateRange(transactionWithRemoveCategory);
                var categoryForRemove = new TransactionCategory()
                {
                    TransactionCategoryId = categoryId
                };

                context.TransactionCategories.Attach(categoryForRemove);
                context.TransactionCategories.Remove(categoryForRemove);
                await context.SaveChangesAsync();

                return await GetTransactionCategories(userId);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}