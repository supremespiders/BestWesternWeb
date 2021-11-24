using System.Threading.Tasks;
using DataAccess.Models;

namespace BestWesternWeb.Models
{
    public interface IHubClient
    {
        Task ScraperStatus(bool isWorking);
        Task Log(Log log);
        Task Error(string message);
        Task Display(string message);
        Task Progress(int percent,int total);
    }
}