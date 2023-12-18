namespace Application.Filters
{
    using System;

    public class TotalReportFilter
    {
        public int CurrencyId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
