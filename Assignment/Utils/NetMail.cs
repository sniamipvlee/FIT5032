using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace Assignment.Utils
{
    public class EmailSender
    {
        public void Send(String toEmail, String subject, String date, int no)
        {
            string path = HttpContext.Current.Server.MapPath(@"~/Utils/Details.txt");
            StreamWriter file = new StreamWriter(path);
            file.WriteLine("Your request comes from: " + toEmail);
            file.WriteLine("The date of the appointment is: " + date);
            file.WriteLine("The serial number is: " + no);
            file.Flush();
            file.Close();

            string smtpServer = "smtp.gmail.com";
            string FromEmail = "871083549zhxt@gmail.com";
            string Password = "hunan3721";
            string contents = "From: " + toEmail
                  + "<p>Date: " + date + "</p>"
                  + "<p>The serial number is: " + no + "</p>";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = smtpServer;
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(FromEmail, Password);

            MailMessage mailMessage = new MailMessage(FromEmail, toEmail);
            mailMessage.Subject = subject;
            mailMessage.Body = contents;
            mailMessage.IsBodyHtml = true;

            Attachment newAttachment = new Attachment(path);
            mailMessage.Attachments.Add(newAttachment);

            smtpClient.Send(mailMessage);
            mailMessage.Dispose();
        }

    }
}