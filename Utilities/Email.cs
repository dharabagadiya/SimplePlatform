
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace Utilities
{
    public class Email
    {
        public delegate void Invoke(string toEmailID, string CCEmailID, string subject, string content);

        private static string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString();
        private static int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpServerPort"].ToString());
        private static string fromEmailID = System.Configuration.ConfigurationManager.AppSettings["fromEmailID"].ToString();
        private static string fromEmailIDPassword = System.Configuration.ConfigurationManager.AppSettings["fromEmailPassword"].ToString();
        private static string mailSubject = System.Configuration.ConfigurationManager.AppSettings["mailSubject"].ToString();

        public void SendMail(string toEmailID, string CCEmailID, string subject, string content)
        {
            Thread.Sleep(100);
            var ccEmailIDs = System.Configuration.ConfigurationManager.AppSettings["ccEMailIDs"].ToString().Split('|');
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmailID);
                mail.To.Add(toEmailID);
                if (!string.IsNullOrEmpty(CCEmailID)) { mail.CC.Add(new MailAddress(CCEmailID)); }
                if (ccEmailIDs.Length > 0)
                {
                    foreach (var ccEmailID in ccEmailIDs) { mail.CC.Add(new MailAddress(ccEmailID)); }
                }
                mail.Subject = subject;
                mail.Body = content;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.Credentials = new NetworkCredential(fromEmailID, fromEmailIDPassword);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = false;
                    smtp.Send(mail);
                }
            }
        }
    }
}
