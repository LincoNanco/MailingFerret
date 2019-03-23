using System.Threading.Tasks;

namespace MailingFerret.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}