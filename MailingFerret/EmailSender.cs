using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailingFerret.Interfaces;

namespace MailingFerret
{
    /// <summary>
    /// Sends emails using the configured credentials
    /// </summary>
    public class EmailSender : IEmailSender
    {


        private readonly SmtpClient _client;
        private readonly IViewRenderService _emailRenderer;

        private readonly EmailSenderSettings _settings;

        public EmailSender(EmailSenderSettings settings, IViewRenderService renderService)
        {
            _settings = settings;
            _emailRenderer = renderService;
            _client = new SmtpClient(_settings.MailHost);
            _client.UseDefaultCredentials = false;
            _client.Credentials = new NetworkCredential(_settings.MailUser, _settings.MailPassword);
        }

        private MailMessage BuildMailMessage(string subject, string message)
        {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(_settings.MailAccount);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                return mailMessage;
        }
        private Task SendEmail(MailMessage mailMessage)
        {
            _client.Send(mailMessage);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Sends an email o the specified recipient
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildMailMessage(subject,message);
                mailMessage.To.Add(email);
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email message to the specified recipients
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildMailMessage(subject, message);
                foreach(string email in emails)
                {   
                    mailMessage.To.Add(email);
                }
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email message to the specified recipients and copied recipients
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="copyEmails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(List<string> emails, List<string> copyEmails, string subject, string message)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildMailMessage(subject, message);
                foreach(string email in emails)
                {   
                    mailMessage.To.Add(email);
                }
                foreach(string email in copyEmails)
                {
                    mailMessage.CC.Add(email);
                }
                SendEmail(mailMessage);
            });
        }
    }
}