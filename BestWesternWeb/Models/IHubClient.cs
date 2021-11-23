using System.Threading.Tasks;

namespace BestWesternWeb.Models
{
    public interface IHubClient
    {
        Task ScraperStatus(bool isWorking);
        Task Log(string message);
        Task Error(string message);
        Task Display(string message);
        Task Progress(int percent,int total);
    }
}