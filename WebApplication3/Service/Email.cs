using System.Net;
using System.Net.Mail;


namespace WebApplication3.Service
{
    public class Email
    {
        public static string HtmlMessage(string verificationCode, string fullName)
        {
            string htmlBody = @"<html><head>";
            htmlBody += @"<style>";
            htmlBody += @"body{ font-family:'Calibri',sans-serif; }";
            htmlBody += @"</style>";
            htmlBody += @"</head><body>";
            htmlBody += @"Hi " + fullName + " please confirm your email address, your verification code is: " + verificationCode;
            htmlBody += @"<h5>Fresh Frame Market</h5>";
            htmlBody += @"</body></html>";
            return htmlBody;
        }
        public static bool SendMail(string receiverEmail, string subject, string htmlBody)
        {
            string Mail = AppSettingsHelper.Setting("MailSettings:Mail");
            string DisplayName = "Administrator";
            string Password = AppSettingsHelper.Setting("MailSettings:Password");
            string Host = "smtp.gmail.com";
            int Port = 587;
            //email config
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(Mail, DisplayName);
                mail.To.Add(receiverEmail);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = htmlBody;

                using (SmtpClient smtp = new SmtpClient(Host, Port))
                {
                    smtp.Credentials = new NetworkCredential(Mail!, Password!);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return true;
        }
    }
}
