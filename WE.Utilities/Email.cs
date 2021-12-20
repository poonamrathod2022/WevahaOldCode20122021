using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace WE.Utilities
{
    public class Email
    {
        public int SentEmail(string FromEmailAddress, string ToEmailAddress, string Subject, string MessageBody)
        {
            MailMessage msg;
            //Sending activation link in the email
            msg = new MailMessage();

            SmtpClient smtp = new SmtpClient();

            msg.From = new MailAddress(FromEmailAddress);
            //Receiver email address
            if (FromEmailAddress == "registration@wevaha.com")
            {
                msg.CC.Add(FromEmailAddress);
            }
            msg.To.Add(ToEmailAddress);
            msg.Subject = Subject;
            msg.Body = MessageBody;
            msg.IsBodyHtml = true;
            smtp.Credentials = new NetworkCredential(FromEmailAddress, "Levan0422");
            smtp.Port = 587;
            smtp.Host = "mail.wevaha.com";
            //smtp.EnableSsl = true;
            try
            {
                smtp.Send(msg);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
