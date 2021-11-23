using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ScraperLib.Models;

namespace ScraperLib.Services
{
    public class MainWorker
    {
        private BestWesternClient _client;
        private int _alertsClosed;
        private int _responses;
        private string _thanks = "";
        private string _apology = "";

        private async Task<List<Input>> ReadInputs(string fileName)
        {
            if (!File.Exists(fileName))
                throw new KnownException($"File {fileName} not found");
            var lines = (await File.ReadAllLinesAsync(fileName)).Distinct().ToList();
            lines.Remove("");
            var inputs = lines.Select(x =>
            {
                var c = x.Split(",");
                return new Input { Username = c[0], Password = c[1] };
            }).ToList();
            Notifier.Display($"{inputs.Count} inputs");
            return inputs;
        }

        public async Task Run(CancellationToken ct=new CancellationToken())
        {
            Notifier.Display("Started Main work");
            var inputs = await ReadInputs("Data/inputs.csv");
            for (var i = 0; i < inputs.Count; i++)
            {
                var input = inputs[i];
                Notifier.Progress(i+1,inputs.Count);
                await Work(input, ct);
            }

            Notifier.Display("Completed");
        }

        private void InitParams()
        {
            _apology = "";
            _thanks = "";
            _alertsClosed = 0;
            _responses = 0;
        }

        private async Task Work(Input input,CancellationToken ct=new CancellationToken())
        {
            InitParams();
            _client = new BestWesternClient(input.Username, input.Password,ct);
            try
            {
                await _client.Login();
            }
            catch (KnownException e)
            {
                Notifier.Error(e.Message);
                return;
            }

            var reviewsResponse = await _client.GetReviews();
            foreach (var feedbackNode in reviewsResponse.data.feedback.nodes)
            {
                await HandleReview(feedbackNode);
            }

            Notifier.Display($"Responses : {_responses} , alerts closed {_alertsClosed}");
        }


        private async Task HandleAlert(ReviewsResponse.Alert alert)
        {
            if (alert == null) return;
            if (!alert.status.id.Equals("NEW")) return;
            try
            {
                await _client.CloseAlert(alert.id);
                _alertsClosed++;
            }
            catch (Exception e)
            {
                Notifier.Error($"Error closing alert : {alert.id} : {e}");
            }
        }

        private async Task HandleReview(ReviewsResponse.Node feedbackNode)
        {
            if (feedbackNode.isReplied) return;
            await HandleAlert(feedbackNode.alert);
            if (feedbackNode.socialSource != null)
            {
                await _client.CloseOnSocial(feedbackNode.id);
                _responses++;
                return;
            }

            if (_thanks == "")
                (_thanks, _apology) = await _client.GetTemplateId(feedbackNode.id);
            if (_thanks.Equals(""))
                return;
            var guestName = feedbackNode.fieldDataList.FirstOrDefault(x => x.field.id.Equals("k_bw_guestname_corpname"))?.values.First();
            var score = double.Parse(feedbackNode.fieldDataList.FirstOrDefault(x => x.field.id.Equals("a_overall_score_with_social_media_native"))?.values.First() ?? "0");
            var response = await _client.GetMessage(feedbackNode.id, score >= 7 ? _thanks : _apology);
            try
            {
                await _client.Reply(feedbackNode.id, score >= 7 ? _thanks : _apology, response);
                _responses++;
            }
            catch (Exception e)
            {
                Notifier.Error(e.Message);
            }
        }
    }
}