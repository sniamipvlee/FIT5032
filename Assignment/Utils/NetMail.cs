using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Assignment.Utils
{
    public class EmailSender
    {
        public void Send(String toEmail, String subject, String date, String name,String customer)
        {
            string path = HttpContext.Current.Server.MapPath(@"~/Utils/Details.pdf");
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            Document doc = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);
            doc.Open();
            doc.Add(new Paragraph("Your request comes from: " + customer));
            doc.Add(new Paragraph("The date of the appointment is: " + date));
            doc.Add(new Paragraph("This customer requests for : " + name));
            doc.Close();
            writer.Close();

            string smtpServer = "smtp.gmail.com";
            string FromEmail = "871083549zhxt@gmail.com";
            string Password = "hunan3721";
            string contents = "From: " + customer
                  + "<p>Date: " + date.Split(' ')[0] + "</p>"
                  + "<p>This customer requests for : " + name + "</p>";

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