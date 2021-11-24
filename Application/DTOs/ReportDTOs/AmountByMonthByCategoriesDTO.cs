using System.Collections.Generic;

namespace Application.DTOs.ReportDTOs
{
    public class AmountByMonthByCategoriesDTO
    {
        public List<float> Data { get; set; }

        public string Label { get; set; }

        public int BorderWidth { get; set; }
    }
}
