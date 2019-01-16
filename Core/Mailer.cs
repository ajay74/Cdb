using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Mail;


namespace Courby.Core.Mail
{
    public static class Mailer
    {
        static System.Net.Mail.SmtpClient _client = new System.Net.Mail.SmtpClient();

        /// <summary>
        /// Sends a simple text message
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public static void SendEmail (string to, string from, string subject, string message)
        {
            SendEmail(CreateMessage(to, from, subject, message, false));
        }
        /// <summary>
        ///  Sends a email message
        /// </summary>
        /// <param name="message"></param>
        public static void SendEmail(MailMessage message)
        {
            _client.Send(message);
        }

        /// <summary>
        /// Creates a standard MailMessage for sending.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        public static MailMessage CreateMessage(string to, string from, string subject, string message, bool isHtml)
        {
            MailMessage mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml
            };

            return mailMessage;
        }

        /// <summary>
        /// Creates a MailMessage from a database template
        /// </summary>
        /// <param name="templateName">Name of the template</param>
        /// <param name="culture">language</param>
        /// <param name="templateKeywords">keywords to replace in the email template</param>
        /// <returns></returns>
        public static MailMessage CreateMessage(string templateName, string culture, Dictionary<string, string> templateKeywords)
        {
            Dictionary<string, string> template = Courby.Resource.Email.EmailResource.GetEmailResouce(templateName, culture);
            MailMessage mailMessage = CreateMessage("", "", template["subject"], template["body"], true);
            List<string> keys = new List<string> (templateKeywords.Keys);


            for (int i = 0; i < templateKeywords.Count; i++)
            {
                mailMessage.Body.Replace(keys[i], templateKeywords[keys[i]]);
            }

            return mailMessage;
        }
        /// <summary>
        /// Create a message from a database template and adds the to and from fields.
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="templateName"></param>
        /// <param name="culture"></param>
        /// <param name="templateKeywords"></param>
        /// <returns></returns>
        public static MailMessage CreateMessage(string to, string from, string templateName, string culture, Dictionary<string, string> templateKeywords)
        {
            MailMessage mailMessage = CreateMessage(templateName, culture, templateKeywords);

            mailMessage.To.Add(to);
            mailMessage.From = new MailAddress(from);

            return mailMessage;
        }
    }
}
