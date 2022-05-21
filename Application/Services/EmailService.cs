namespace Application.Services
{
    using MailKit.Net.Smtp;
    using MimeKit;
    using System;
    using System.Threading.Tasks;

    public static class EmailService
    {
        public static async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Money Manager System", "m0neymanagermail@yandex.by"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.yandex.ru", 465, true);
                await client.AuthenticateAsync("m0neymanagermail@yandex.by", EmailPasswordContainer.GetInstance().Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
