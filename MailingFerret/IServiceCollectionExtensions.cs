using MailingFerret.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MailingFerret
{
    public static class IServiceCollectionExtensions
    {
        public static void AddMailingFerret(this IServiceCollection services, string MailHost, string MailUser, string MailPassword, string MailAccount)
        {
            services.AddSingleton(new EmailSenderSettings(MailHost,MailUser,MailPassword,MailAccount));
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}