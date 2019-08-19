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
        private readonly IViewRenderService _emailRenderer;
        readonly IConfiguration _configuration;

        public EmailSender(IViewRenderService renderService, IConfiguration configuration)
        {
            _configuration = configuration;
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailHost"])) throw new ArgumentException(nameof(MailHost));
            MailHost = configuration["MailingFerret:EmailHost"];
            if (string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailAccount"])) throw new ArgumentException(nameof(MailAccount));
            MailAccount = configuration["MailingFerret:EmailAccount"];
            TemplatesLocation = configuration["MailingFerret:TemplatesLocation"];
            _emailRenderer = renderService;
            
            //network credentials are optional now
            if (!string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailUser"])) MailUser = configuration["MailingFerret:EmailUser"];
            if (!string.IsNullOrWhiteSpace(configuration["MailingFerret:EmailPassword"])) MailPassword = configuration["MailingFerret:EmailPassword"];
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient(MailHost);
            client.UseDefaultCredentials = false;

            //setting network credentials only if they are defined
            if (!string.IsNullOrEmpty(MailUser))
            {
                client.Credentials = new NetworkCredential(MailUser, MailPassword);
            }

            if (!string.IsNullOrWhiteSpace(_configuration["MailingFerret:Port"]))
            {
                client.Port = Convert.ToInt32(_configuration["MailingFerret:Port"]);
            }
            if (!string.IsNullOrWhiteSpace(_configuration["MailingFerret:EnableSsl"]))
            {
                client.EnableSsl = Convert.ToBoolean(_configuration["MailingFerret:EnableSsl"]);
            }
            return client;
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
            using (SmtpClient client = GetSmtpClient())
            {
                client.Send(mailMessage);
            }
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