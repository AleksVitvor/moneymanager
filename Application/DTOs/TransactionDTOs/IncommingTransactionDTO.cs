using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.TransactionDTOs
{
    public class IncommingTransactionDTO
    {
        public int Id { get; set; }

        public float StoredAmount { get; set; }

        public bool IsRepeatable { get; set; }

        public int TransactionTypeId { get; set; }

        public int TransactionCategoryId { get; set; }

        public DateTime TransactionDate { get; set; }

        public int UserId { get; set; }
    }
}
