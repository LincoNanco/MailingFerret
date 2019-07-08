using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MailingFerret.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MailingFerret
{
    /// <summary>
    /// Sends emails using the configured credentials
    /// </summary>
    public class EmailSender : IEmailSender
    {
        public readonly string MailHost;
        public readonly string MailUser;
        public readonly string MailPassword;
        public readonly string MailAccount;
        public readonly string TemplatesLocation;
        private readonly SmtpClient _client;
        private readonly IViewRenderService _emailRenderer;

        public EmailSender(IViewRenderService renderService, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailHost"])) throw new ArgumentException(nameof(MailHost));
            MailHost = configuration["MailingFerret:EmailHost"];
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailUser"])) throw new ArgumentException(nameof(MailUser));
            MailUser = configuration["MailingFerret:EmailUser"];
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailPassword"])) throw new ArgumentException(nameof(MailPassword));
            MailPassword = configuration["MailingFerret:EmailPassword"];
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailAccount"])) throw new ArgumentException(nameof(MailAccount));
            MailAccount = configuration["MailingFerret:EmailAccount"];
            TemplatesLocation = configuration["MailingFerret:TemplatesLocation"];
            _emailRenderer = renderService;
            _client = new SmtpClient(MailHost);
            _client.UseDefaultCredentials = false;
            _client.Credentials = new NetworkCredential(MailUser, MailPassword);

            if (!string.IsNullOrWhiteSpace(configuration["MailingFerret:Port"]))
            {
                _client.Port = Convert.ToInt32(configuration["MailingFerret:Port"]);
            }
            if (!string.IsNullOrWhiteSpace(configuration["MailingFerret:EnableSsl"]))
            {
                _client.EnableSsl = Convert.ToBoolean(configuration["MailingFerret:EnableSsl"]);
            }
        }

        private MailMessage BuildEmailMessage(string subject, string message)
        {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(MailAccount);
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
        /// Sends an email to the specified recipient.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildEmailMessage(subject,message);
                mailMessage.To.Add(email);
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email to the specified recipient using a Razor template.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string templateName, object model)
        {
            string message = _emailRenderer.RenderToStringAsync(templateName, model).Result;
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildEmailMessage(subject, message);
                mailMessage.To.Add(email);
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email message to the specified recipients.
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildEmailMessage(subject, message);
                foreach(string email in emails)
                {   
                    mailMessage.To.Add(email);
                }
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email message to the specified recipients using a Razor email template.
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(List<string> emails, string subject, string templateName, object model)
        {
            string message = _emailRenderer.RenderToStringAsync(templateName, model).Result;
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildEmailMessage(subject, message);
                foreach(string email in emails)
                {   
                    mailMessage.To.Add(email);
                }
                SendEmail(mailMessage);
            });
        }

        /// <summary>
        /// Sends an email message to the specified recipients and copied recipients.
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
                MailMessage mailMessage = BuildEmailMessage(subject, message);
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

              /// <summary>
        /// Sends an email message to the specified recipients and copied recipients using a Razor email template.
        /// </summary>
        /// <param name="emails"></param>
        /// <param name="copyEmails"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendEmailAsync(List<string> emails, List<string> copyEmails, string subject, string templateName, object model)
        {
            string message = _emailRenderer.RenderToStringAsync(templateName, model).Result;
            return Task.Run(() =>
            {
                MailMessage mailMessage = BuildEmailMessage(subject, message);
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