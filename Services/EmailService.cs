using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;


namespace LoginAPI.Services
{

    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public SendGridEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var apiKey = _configuration["SendGrid:ApiKey"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_configuration["SendGrid:SenderEmail"], _configuration["SendGrid:SenderName"]);
                var to = new EmailAddress(toEmail);
                var plainTextContent = message;
                var htmlContent = message;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

                var response = await client.SendEmailAsync(msg);

                Console.WriteLine($"SendGrid Response Status Code: {response.StatusCode}");
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"SendGrid Response Body: {responseBody}");

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    Console.WriteLine($"Error sending email: {response.StatusCode} - {responseBody}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while sending email: {ex.Message}");
            }
        }

    }

}
