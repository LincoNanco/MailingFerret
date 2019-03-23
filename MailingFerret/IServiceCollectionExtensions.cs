using MailingFerret.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MailingFerret
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="MailHost"></param>
        /// <param name="MailUser"></param>
        /// <param name="MailPassword"></param>
        /// <param name="MailAccount"></param>
        public static void AddMailingFerret(this IServiceCollection services)
        {
            //Add an implementation of IViewRenderService only if it is not already provided
            services.TryAddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}