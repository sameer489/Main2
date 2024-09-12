using System.Net.Mail;
using System.Net;
using System.Text;

namespace Sending_Emails_in_Asp.Net_Core
{
    public class EmailSending 
    {
        public void SendEmail(string toEmail, string username)
        {
            // Set up SMTP client
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("sameerskars@gmail.com", "ukgq mmxp dezu zbcy");

            // Create email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sameerskars@gmail.com");
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Regarding to Flight Booking Application";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>Registration successful</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat($"Welcome to Flight Booking Account {username}");
            mailBody.AppendFormat("<p>Thank you For Registering account</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}