using System.Net.Mail;
using OnlineShop.Exceptions;

namespace OnlineShop.Services
{ 
    public class EmailService
    {
        public void SendRecoveryCode(string email, int recoveryCode)
        {
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress("autodilyobchod@gmail.com"),
                    Subject = "Recovery code",
                    Body = $"Your recovery code: {recoveryCode}"
                };
                mail.To.Add(email);

                using var smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("autodilyobchod@gmail.com", "kzrr ynuc iati hlif"),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {             
                throw new EmailSendException("Error occurred while sending recovery code.", ex);
            }
            catch (Exception ex)
            {
                throw new EmailSendException("An unexpected error occurred while sending recovery code.", ex);
            }
        }
    }
}
