namespace MoneyManager.Extensions
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using System;

    public static class EnvironmentVariableExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                return Environment.GetEnvironmentVariable("MONEYMANAGER_CONNECTION_STRING", EnvironmentVariableTarget.Process);
            }

            return configuration["MoneyManager:ConnectionStrings:MoneyManagerDB"];
        }

        public static string GetBillRecognizerEndpoint(this IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                return Environment.GetEnvironmentVariable("BILL_RECOGNIZER_ENDPOINT", EnvironmentVariableTarget.Process);
            }

            return configuration["MoneyManager:ConnectionStrings:BillRecognizerEndpoint"];
        }

        public static string GetBillRecognizerApiKey(this IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                return Environment.GetEnvironmentVariable("BILL_RECOGNIZER_APIKEY", EnvironmentVariableTarget.Process);
            }

            return configuration["MoneyManager:ConnectionStrings:BillRecognizerApiKey"];
        }

        public static string GetEmailPassword(this IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsProduction())
            {
                return Environment.GetEnvironmentVariable("EMAIL_PASSWORD", EnvironmentVariableTarget.Process);
            }

            return configuration["MoneyManager:Passwords:YandexMailPassword"];
        }
    }

}
