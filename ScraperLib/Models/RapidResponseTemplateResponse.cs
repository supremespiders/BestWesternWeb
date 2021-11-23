namespace BestWesternBot.Models
{
    public class RapidResponseTemplateResponse
    {
        public Data data { get; set; }

        public class Data
        {
            public Rapidresponsetemplate rapidResponseTemplate { get; set; }
        }

        public class Rapidresponsetemplate
        {
            public string id { get; set; }
            public string name { get; set; }
            public string emailSubject { get; set; }
            public string emailBody { get; set; }
            public string emailTo { get; set; }
            public bool emailCcEnabled { get; set; }
            public string[] emailCc { get; set; }
            public object[] emailCcFixed { get; set; }
            public bool supportsMarkup { get; set; }
        }

    }
}