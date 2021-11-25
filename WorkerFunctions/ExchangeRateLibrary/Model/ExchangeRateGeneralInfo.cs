using System;
using System.Collections.Generic;

namespace ExchangeRateLibrary.Model
{
    public class ExchangeRateGeneralInfo
    {
        public bool Success { get; set; }

        public string Base { get; set; }

        public DateTime Date { get; set; }

        public CurrencyExchangeRate Rates { get; set; }
    }
}
