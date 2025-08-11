using System.Net;
using System.Net.Mail;

namespace Application.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("helhafresh@gmail.com", "xlwd jsbf xlue mfix"),
                EnableSsl = true
            };
            
            var mail = new MailMessage("helhafresh@gmail.com", to, subject, body);

            await client.SendMailAsync(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur envoi email : {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}