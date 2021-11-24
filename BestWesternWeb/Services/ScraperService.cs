using System;
using System.Threading;
using System.Threading.Tasks;
using BestWesternWeb.Models;
using DataAccess.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ScraperLib.Models;
using ScraperLib.Services;

namespace BestWesternWeb.Services
{
    public class ScraperService
    {
        public bool IsWorking
        {
            get => _isWorking;
            set
            {
                _hubContext.Clients.All.ScraperStatus(value).GetAwaiter().GetResult();
                _isWorking = value;
            }
        }

        private MainWorker _worker;
        private CancellationTokenSource _cancellation;
        private readonly ILogger<ScraperService> _logger;
        private readonly IHubContext<BroadcastHub, IHubClient> _hubContext;
        private bool _isWorking;

        public ScraperService(ILogger<ScraperService> logger, IHubContext<BroadcastHub, IHubClient> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            Notifier.OnLog += OnLog;
            Notifier.OnDisplay += OnDisplay;
            Notifier.OnProgress += OnProgress;
            Notifier.OnError += OnError;
        }

        private async void OnError(object sender, string e)
        {
            var log = new Log { Id = 1, Message = e, LogLevel = "Error", TimeStamp = DateTime.Now };
            await _hubContext.Clients.All.Log(log);
            await _hubContext.Clients.All.Error(e);
        }

        private async void OnProgress(object sender, (int x, int total) e)
        {
            await _hubContext.Clients.All.Progress(e.x, e.total);
        }

        private async void OnDisplay(object sender, string e)
        {
            await _hubContext.Clients.All.Display(e);
        }

        private async void OnLog(object sender, string e)
        {
            var log = new Log { Id = 1, Message = e, LogLevel = "Info", TimeStamp = DateTime.Now };
            await _hubContext.Clients.All.Log(log);
            _logger.LogInformation(e);
        }

        private async Task CleanLogs()
        {
            
        }

        public async Task Start()
        {
            
            // _logger.LogInformation("this is a good piece if information");
            // _logger.LogError("Exception : this is a very big exception with lines wrap\nits called on one of the line i don't know\nmaybe very long a lot long");
            IsWorking = true;
            _worker = new MainWorker();
            _cancellation = new CancellationTokenSource();
            try
            {
                await _worker.Run(_cancellation.Token);
            }
            catch (TaskCanceledException)
            {
                Notifier.Display("Canceled by user");
                Notifier.Progress(100, 100);
                _logger.LogDebug("Canceled by user");
            }
            catch (KnownException ex)
            {
                Notifier.Error(ex.Message);
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                Notifier.Error(ex.ToString());
                _logger.LogError($"critical exception {ex}");
            }
            finally
            {
                IsWorking = false;
            }
        }

        public void Stop()
        {
            _cancellation?.Cancel();
        }
    }
}