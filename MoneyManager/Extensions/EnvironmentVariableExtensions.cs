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
    }

}
