﻿namespace Application.Services.TransactionService
{
    using Application.DTOs.TransactionDTOs;
    using AutoMapper;
    using Azure;
    using Azure.AI.FormRecognizer.DocumentAnalysis;
    using Domain;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class TransactionService : BaseService, ITransactionService
    {
        public TransactionService(MoneyManagerContext context, IMapper mapper)
            :base(context, mapper)
        {
        }

        public async Task<List<TransactionDTO>> GetTransactions(int userId)
        {
            try
            {
                var transactions = await context.Transactions
                    .Include(x => x.TransactionCategory)
                    .Include(x => x.TransactionType)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.TransactionDate)
                    .ToListAsync();
                return mapper.Map<List<Transaction>, List<TransactionDTO>>(transactions);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> CreateTransaction(IncommingTransactionDTO transactionDTO)
        {
            try
            {
                await context.Transactions.AddAsync(mapper.Map<IncommingTransactionDTO, Transaction>(transactionDTO));
                await context.SaveChangesAsync();
                return await GetTransactions(transactionDTO.UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> UpdateTransaction(IncommingTransactionDTO transactionDTO)
        {
            try
            {
                context.Transactions.Update(mapper.Map<IncommingTransactionDTO, Transaction>(transactionDTO));
                await context.SaveChangesAsync();
                return await GetTransactions(transactionDTO.UserId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<TransactionDTO>> DeleteTransaction(int id, int userId)
        {
            try
            {
                Transaction transaction = new() { TransactionId = id };
                var childTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.ParentTransactionId == id);
                if (childTransaction != null)
                {
                    childTransaction.ParentTransactionId = null;
                    context.Transactions.Update(childTransaction);
                }

                var parentTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.ChildTransactionId == id);
                if (parentTransaction != null)
                {
                    parentTransaction.ChildTransactionId = null;
                    context.Transactions.Update(parentTransaction);
                }

                context.Transactions.Attach(transaction);
                context.Transactions.Remove(transaction);
                await context.SaveChangesAsync();
                return await GetTransactions(userId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddPhotoTransaction(int userId, string path)
        {
            try
            {
                var apiKey = BlobConnectionStringContainer.GetInstance().ApiKey;
                var endPoint = BlobConnectionStringContainer.GetInstance().URL;
                var universalTransactionCategory = await context.TransactionCategories.FirstAsync(x => x.Description == "Universal" && x.UserId == userId);
                var expenciesType = await context.TransactionTypes.FirstAsync(x => x.Description == "Expenses");

                var credential = new AzureKeyCredential(apiKey);
                var client = new DocumentAnalysisClient(new Uri(endPoint), credential);

                await using var memoryStream = new MemoryStream(File.ReadAllBytes(path));

                var operation = await client.StartAnalyzeDocumentAsync("prebuilt-receipt", memoryStream);

                await operation.WaitForCompletionAsync();

                var result = operation.Value;

                var newTransactions = new List<Transaction>();

                for (int i = 0; i < result.Documents.Count; i++)
                {
                    var transaction = new Transaction();
                    var document = result.Documents[i];

                    if (document.Fields.TryGetValue("Total", out DocumentField? itemsField))
                    {
                        if (itemsField.ValueType == DocumentFieldType.Double)
                        {
                            transaction.Amount = (float)itemsField.AsDouble();
                            transaction.CurrencyId = (await context.Currencies
                                .AsQueryable()
                                .Where(x => itemsField.Content.Contains(x.CurrencySymbol))
                                .FirstOrDefaultAsync())?.CurrencyId ?? 1; 
                        }
                    }

                    if (document.Fields.TryGetValue("TransactionDate", out DocumentField? transactionDate))
                    {
                        if (transactionDate.ValueType == DocumentFieldType.Date)
                        {
                            transaction.TransactionDate = transactionDate.AsDate();
                        }
                    }

                    transaction.IsRepeatable = false;
                    transaction.TransactionCategoryId = universalTransactionCategory.TransactionCategoryId;
                    transaction.TransactionTypeId = expenciesType.TransactionTypeId;
                    transaction.UserId = userId;

                    newTransactions.Add(transaction);
                }

                await context.Transactions.AddRangeAsync(newTransactions);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
