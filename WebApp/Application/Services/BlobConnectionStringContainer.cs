namespace Application.Services
{
    public class BlobConnectionStringContainer
    {
        private static BlobConnectionStringContainer instance;

        public string ApiKey { get; private set; }

        public string URL { get; private set; }

        private BlobConnectionStringContainer(string apiKey, string URL)
        {
            ApiKey = apiKey;
            this.URL = URL;
        }

        public static void Create(string apiKey, string URL)
        {
            if (instance == null)
            {
                instance = new BlobConnectionStringContainer(apiKey, URL);
            }
        }

        public static BlobConnectionStringContainer GetInstance()
        {
            return instance;
        }
        
    }
}
