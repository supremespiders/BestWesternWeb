namespace ScraperLib.Models
{
    public class ReviewsResponse
    {

        public Data data { get; set; }

        public class Data
        {
            public Feedback feedback { get; set; }
        }

        public class Feedback
        {
            public bool hasConcealedRecords { get; set; }
            public Pageinfo pageInfo { get; set; }
            public Node[] nodes { get; set; }
            public int totalCount { get; set; }
            public string __typename { get; set; }
        }

        public class Pageinfo
        {
            public string endCursor { get; set; }
            public string __typename { get; set; }
        }

        public class Node
        {
            public Activities activities { get; set; }
            public Alert alert { get; set; }
            public string __typename { get; set; }
            public string id { get; set; }
            public bool isExcluded { get; set; }
            public object isRead { get; set; }
            public bool isReplied { get; set; }
            public bool isRepliedOnSocial { get; set; }
            public Fielddatalist[] fieldDataList { get; set; }
            public Commentfielddatalist[] commentFieldDataList { get; set; }
            public object socialSentiment { get; set; }
            public Socialsource socialSource { get; set; }
            public object tag { get; set; }
        }

        public class Activities
        {
            public int totalCount { get; set; }
            public string __typename { get; set; }
        }

        public class Alert
        {
            public string id { get; set; }
            public Actions actions { get; set; }
            public Status status { get; set; }
            public Workflow workflow { get; set; }
            public Allowedstatuschange[] allowedStatusChanges { get; set; }
            public string __typename { get; set; }
        }

        public class Actions
        {
            public object putAlertWorkflow { get; set; }
            public string putAssignAlert { get; set; }
            public string __typename { get; set; }
        }

        public class Status
        {
            public string id { get; set; }
            public string label { get; set; }
            public string __typename { get; set; }
        }

        public class Workflow
        {
            public string id { get; set; }
            public string briefName { get; set; }
            public string __typename { get; set; }
        }

        public class Allowedstatuschange
        {
            public bool active { get; set; }
            public string id { get; set; }
            public string __typename { get; set; }
        }

        public class Socialsource
        {
            public string id { get; set; }
            public string name { get; set; }
            public string kind { get; set; }
            public Overallscore overallScore { get; set; }
            public string __typename { get; set; }
        }

        public class Overallscore
        {
            public Scale scale { get; set; }
            public string __typename { get; set; }
        }

        public class Scale
        {
            public Colorscheme[] colorSchemes { get; set; }
            public int min { get; set; }
            public int max { get; set; }
            public string __typename { get; set; }
        }

        public class Colorscheme
        {
            public string background { get; set; }
            public int end { get; set; }
            public int start { get; set; }
            public string __typename { get; set; }
        }

        public class Fielddatalist
        {
            public string[] labels { get; set; }
            public string[] values { get; set; }
            public Field field { get; set; }
            public string __typename { get; set; }
        }

        public class Field
        {
            public string id { get; set; }
            public string __typename { get; set; }
        }

        public class Commentfielddatalist
        {
            public Field1 field { get; set; }
            public string originalLanguage { get; set; }
            public string processingLanguage { get; set; }
            public Textswithlanguage[] textsWithLanguage { get; set; }
            public string __typename { get; set; }
            public string[] translatableLanguages { get; set; }
            public object[] piiTaggings { get; set; }
            public Ruletopictagging[] ruleTopicTaggings { get; set; }
            public Sentimenttagging[] sentimentTaggings { get; set; }
            public object mediaFileMetadata { get; set; }
        }

        public class Field1
        {
            public string id { get; set; }
            public string __typename { get; set; }
        }

        public class Textswithlanguage
        {
            public string text { get; set; }
            public string language { get; set; }
            public string __typename { get; set; }
        }

        public class Ruletopictagging
        {
            public Topic topic { get; set; }
            public Region[] regions { get; set; }
            public string __typename { get; set; }
        }

        public class Topic
        {
            public string id { get; set; }
            public string name { get; set; }
            public string __typename { get; set; }
        }

        public class Region
        {
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public string __typename { get; set; }
        }

        public class Sentimenttagging
        {
            public Region1[] regions { get; set; }
            public string sentiment { get; set; }
            public string __typename { get; set; }
        }

        public class Region1
        {
            public int startIndex { get; set; }
            public int endIndex { get; set; }
            public string __typename { get; set; }
        }

    }
}