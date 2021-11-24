using System.Linq;
using System.Threading.Tasks;
using BestWesternWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BestWesternWeb.Controllers
{
    public class Scraper : Controller
    {
        // GET: ScrapeController
        private readonly ScraperService _scraperService;
        private readonly DataAccess.DbContext _context;

        public Scraper(ScraperService scraper, DataAccess.DbContext context)
        {
            _scraperService = scraper;
            _context = context;
        }

        public ActionResult Index()
        {
            return Ok("Welcome to Scraper, you can start / cancel");
        }

        public IActionResult GetLogs()
        {
            var logs = _context.Logs.AsNoTracking().ToList();
            return Ok(logs);
        }

        // GET: ScrapeController/Details/5
        public async Task<ActionResult> Start()
        {
            if (_scraperService.IsWorking)
                return Ok(new { msg = "already working" });
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM Logs;");
            _ = _scraperService.Start();
            return Ok(new { msg = "started" });
        }

        public ActionResult Cancel()
        {
            if (!_scraperService.IsWorking)
                return Ok(new { msg = "not working" });
            _scraperService.Stop();
            return Ok(new { msg = "canceled" });
        }
    }
}