namespace Application.Filters
{
    using System;
    using System.Collections.Generic;

    public class CategoryReportRequestFilter
    {
        public List<int> CategoriesList { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime StartDate { get; set; }

        public int CurrencyId { get; set; }
    }
}
