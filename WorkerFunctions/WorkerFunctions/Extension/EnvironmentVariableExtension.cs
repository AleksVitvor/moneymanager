namespace WorkerFunctions.Extension
{
    using System;

    public class EnvironmentVariableExtension
    {
        public static string GetDbConnectionString()
        {
            return Environment.GetEnvironmentVariable("MONEYMANAGER_CONNECTION_STRING",
                EnvironmentVariableTarget.Process);
        }
    }
}
