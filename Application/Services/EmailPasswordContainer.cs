namespace Application.Services
{
    public class EmailPasswordContainer
    {
        private static EmailPasswordContainer instance;

        public string Password { get; private set; }

        private EmailPasswordContainer(string password)
        {
            Password = password;
        }

        public static void Create(string password)
        {
            if (instance == null)
            {
                instance = new EmailPasswordContainer(password);
            }
        }

        public static EmailPasswordContainer GetInstance()
        {
            return instance;
        }
    }
}
