using System.Net.Mail;
using System.Net;
using System.Text;

namespace FlightBooking_CaseStudy_sameer.Sevices
{
    public class EmailBooking
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
            mailMessage.Subject = "Regarding to Flight Booking Status";
            mailMessage.IsBodyHtml = true;
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendFormat("<h1>Regarding Booking Ticket</h1>");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat($"Hi {username} ");
            mailBody.AppendFormat("<br />");
            mailBody.AppendFormat("Your Ticket Booking is Success");
            mailBody.AppendFormat("<p>Thank you For Booking Ticket</p>");
            mailMessage.Body = mailBody.ToString();

            // Send email
            client.Send(mailMessage);
        }
    }
}
