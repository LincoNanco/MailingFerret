using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailingFerret.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailAsync(string email, string subject, string templateName, object model);

        Task SendEmailAsync(List<string> emails, string subject, string message);
        Task SendEmailAsync(List<string> emails, string subject, string templateName, object model);

        Task SendEmailAsync(List<string> emails, List<string> copyEmails, string subject, string message);
        Task SendEmailAsync(List<string> emails, List<string> copyEmails, string subject, string templateName, object model);
    }
}