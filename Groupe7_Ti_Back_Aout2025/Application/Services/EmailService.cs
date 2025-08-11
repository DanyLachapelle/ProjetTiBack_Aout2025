using System.Net;
using System.Net.Mail;

namespace Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string token)
        {
            try
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("helhafresh@gmail.com", "xlwd jsbf xlue mfix"),
                    EnableSsl = true
                };

                string resetUrl = $"http://localhost:4200/reset-password?token={token}";

                string htmlBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f5f5f5; padding: 20px;'>
                    <div style='max-width: 600px; margin: auto; background: white; padding: 20px; border-radius: 8px; text-align: center;'>
                        <img src='https://tonsite.com/logo.png' alt='Logo' style='width: 120px; margin-bottom: 20px;'>
                        <h2 style='color: #333;'>Réinitialisation de votre mot de passe</h2>
                        <p>Bonjour,</p>
                        <p>Nous avons reçu une demande pour réinitialiser votre mot de passe. Cliquez sur le bouton ci-dessous pour continuer.</p>
                        <a href='{resetUrl}' style='display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>
                            Réinitialiser mon mot de passe
                        </a>
                        <p style='margin-top: 20px; font-size: 12px; color: gray;'>Si vous n'avez pas demandé cette réinitialisation, ignorez cet email.</p>
                    </div>
                </body>
                </html>";

                var mail = new MailMessage("helhafresh@gmail.com", to, subject, htmlBody);
                mail.IsBodyHtml = true; // Indique que le corps est du HTML

                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur envoi email : {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
