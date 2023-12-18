namespace Application.DTOs.ReportDTOs
{
    using System;
    using System.Collections.Generic;

    public class TotalReportDTO
    {
        public double Total { get; set; }

        public double Refill { get; set; }

        public double Expenses { get; set; }

        public string Currency { get; set; }

        public string Month { get; set; }

        public DateTime Date { get; set; }

        public List<TotalReportTransactionDTO> MonthTransactions { get; set; }
    }
}
