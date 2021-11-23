using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BestWesternBot.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using ScraperLib.Helpers;
using ScraperLib.Models;

namespace ScraperLib.Services
{
    public class BestWesternClient
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly HttpClient _httpClient;
        private readonly string _user;
        private readonly string _pass;
        private string _csrf;
        private readonly CancellationToken _ct;


        public BestWesternClient(string user, string pass, CancellationToken ct)
        {
            _user = user;
            _pass = pass;
            _ct = ct;
            _httpClient = new HttpClient(new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
        }

        public async Task Login()
        {
            Notifier.Display($"Logging in to {_user}");
            var html = await _httpClient.PostFormData("https://express.medallia.com/reflections/logonSubmit.do", new Dictionary<string, string>
            {
                { "username", _user },
                { "password", _pass },
                { "hasJavaScript", "yes" },
                { "flashVersion", "0.0.0" },
                { "screenResolution", "1536x864" },
                { "colorDepth", "24" },
                { "timeZoneOffset", "60" }
            }, 1);
            _csrf = html.GetStringBetween("csrfToken: \"", "\"");
            if (_csrf == null)
                throw new KnownException($"Failed to login");
            Notifier.Display("Logged in");
        }

        public async Task<ReviewsResponse> GetReviews()
        {
            Notifier.Display("Getting reviews");
            var reviewResponse = (await PostJson("https://express.medallia.com/api-comp/reporting/query?view_as_role=51216", "{\"operationName\":\"getResponses\",\"variables\":{\"hasCommentFields\":true,\"hasKeywordSearch\":false,\"after\":\"0\",\"commentFieldIds\":[\"q_socialmedia_comment\",\"q_bw_improve_reward_elite_member_recognition_cmt\",\"q_bw_improve_breakfast_experience_cmt\",\"q_h_bw_staff_next_stay_better_2018\",\"q_restaurant_lounge_experience_cmt\",\"q_make_stay_enjoyable_cmt\"],\"dataView\":{\"id\":\"52\"},\"expandSocial\":true,\"fieldIds\":[\"a_unit_identifier\",\"a_overall_score_with_social_media_native\",\"k_bw_guestname_corpname\",\"k_bw_surveyresponse_or_socialcreation_datetime\",\"q_ov_service\",\"a_overall_score_with_social_media\",\"a_overall_score_with_social_media_color\"],\"filter\":{\"and\":[{\"or\":[{\"fieldIds\":[\"a_social_media_source_enum::seqnum\"],\"isNull\":true},{\"fieldIds\":[\"a_social_media_source_enum::seqnum\"],\"in\":[\"21\",\"2\",\"14\",\"3\",\"7\",\"19\",\"1\",\"22\",\"17\",\"13\",\"4\",\"9\",\"5\",\"0\",\"8\",\"20\"]}]},{\"fieldIds\":[\"e_responsedate\"],\"gte\":\"" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "\",\"lt\":\"" + DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "\"}]},\"includeAdHoc\":false,\"search\":[],\"limit\":100,\"orderBy\":[{\"fieldId\":\"e_responsedate\",\"direction\":\"DESC\"},{\"fieldId\":\"a_surveyid\",\"direction\":\"DESC\"}],\"showExcluded\":false},\"query\":\"query getResponses($after: ID, $commentFieldIds: [ID!]!, $dataView: DataView, $expandSocial: Boolean!, $fieldIds: [ID!]!, $filter: Filter, $hasCommentFields: Boolean = false, $hasKeywordSearch: Boolean = false, $includeAdHoc: Boolean!, $search: [String!]!, $limit: Int!, $orderBy: [RecordOrder], $showExcluded: Boolean!) {\\n  feedback(after: $after, dataView: $dataView, filter: $filter, first: $limit, includeAdHoc: $includeAdHoc, orderBy: $orderBy, showExcluded: $showExcluded) {\\n    hasConcealedRecords\\n    pageInfo {\\n      endCursor\\n      __typename\\n    }\\n    nodes {\\n      activities {\\n        totalCount\\n        __typename\\n      }\\n      ...alertFragment\\n      id\\n      isExcluded\\n      isRead\\n      isReplied\\n      isRepliedOnSocial\\n      fieldDataList(fieldIds: $fieldIds, filterUnanswered: true) {\\n        labels\\n        values\\n        field {\\n          id\\n          __typename\\n        }\\n        ... on CommentFieldData {\\n          ...commentData\\n          ...KeywordSearchLanguagesFragment\\n          __typename\\n        }\\n        __typename\\n      }\\n      commentFieldDataList: fieldDataList(fieldIds: $commentFieldIds, filterUnanswered: true) @include(if: $hasCommentFields) {\\n        ... on CommentFieldData {\\n          ...commentData\\n          ...KeywordSearchLanguagesFragment\\n          ...mediaFileMetadataFragment\\n          __typename\\n        }\\n        __typename\\n      }\\n      socialSentiment @include(if: $expandSocial) {\\n        id\\n        value\\n        actions {\\n          putUpdateSocialSentiment\\n          __typename\\n        }\\n        __typename\\n      }\\n      socialSource @include(if: $expandSocial) {\\n        id\\n        name\\n        kind\\n        overallScore {\\n          scale {\\n            colorSchemes {\\n              background\\n              end\\n              start\\n              __typename\\n            }\\n            min\\n            max\\n            __typename\\n          }\\n          __typename\\n        }\\n        __typename\\n      }\\n      tag {\\n        id\\n        label\\n        __typename\\n      }\\n      __typename\\n    }\\n    totalCount\\n    __typename\\n  }\\n}\\n\\nfragment commentData on CommentFieldData {\\n  field {\\n    id\\n    __typename\\n  }\\n  ...LanguagesFragment\\n  translatableLanguages\\n  ...PIITaggingsFragment\\n  __typename\\n}\\n\\nfragment PIITaggingsFragment on CommentFieldData {\\n  piiTaggings {\\n    region {\\n      startIndex\\n      endIndex\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment LanguagesFragment on CommentFieldData {\\n  originalLanguage\\n  processingLanguage\\n  textsWithLanguage {\\n    text\\n    language\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment alertFragment on FeedbackRecord {\\n  alert {\\n    id\\n    actions {\\n      putAlertWorkflow\\n      putAssignAlert\\n      __typename\\n    }\\n    status {\\n      id\\n      label\\n      __typename\\n    }\\n    workflow {\\n      id\\n      briefName\\n      __typename\\n    }\\n    allowedStatusChanges {\\n      active\\n      id\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment mediaFileMetadataFragment on CommentFieldData {\\n  ruleTopicTaggings {\\n    topic {\\n      id\\n      name\\n      __typename\\n    }\\n    regions {\\n      startIndex\\n      endIndex\\n      __typename\\n    }\\n    __typename\\n  }\\n  ...SentimentTaggingsFragment\\n  mediaFileMetadata {\\n    livingLensReport\\n    mediaProcessingStatus\\n    ... on VideoFileMetadata {\\n      thumbnailUrl\\n      fileVersions {\\n        fileUrl\\n        quality\\n        format\\n        __typename\\n      }\\n      durationMillis\\n      transcripts {\\n        language\\n        cuePoints {\\n          lines {\\n            voice\\n            persona\\n            startTextIndex\\n            endTextIndex\\n            __typename\\n          }\\n          startTextIndex\\n          endTextIndex\\n          startTimeMilliseconds\\n          endTimeMilliseconds\\n          __typename\\n        }\\n        vtt\\n        __typename\\n      }\\n      mediaAnalytics {\\n        waitTimeMillis\\n        silenceTimeMillis\\n        voiceChannels {\\n          voice\\n          role\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    ... on AudioFileMetadata {\\n      fileVersions {\\n        fileUrl\\n        quality\\n        format\\n        __typename\\n      }\\n      durationMillis\\n      showDownloadLink\\n      transcripts {\\n        language\\n        cuePoints {\\n          lines {\\n            voice\\n            persona\\n            startTextIndex\\n            endTextIndex\\n            __typename\\n          }\\n          startTextIndex\\n          endTextIndex\\n          startTimeMilliseconds\\n          endTimeMilliseconds\\n          __typename\\n        }\\n        vtt\\n        __typename\\n      }\\n      mediaAnalytics {\\n        waitTimeMillis\\n        silenceTimeMillis\\n        voiceChannels {\\n          voice\\n          role\\n          __typename\\n        }\\n        __typename\\n      }\\n      __typename\\n    }\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment SentimentTaggingsFragment on CommentFieldData {\\n  sentimentTaggings {\\n    regions {\\n      startIndex\\n      endIndex\\n      __typename\\n    }\\n    sentiment\\n    __typename\\n  }\\n  __typename\\n}\\n\\nfragment KeywordSearchLanguagesFragment on CommentFieldData {\\n  keywordSearchMatchesWithLanguages(search: $search) @include(if: $hasKeywordSearch) {\\n    matches {\\n      source\\n      regions {\\n        startIndex\\n        endIndex\\n        __typename\\n      }\\n      token\\n      __typename\\n    }\\n    language\\n    __typename\\n  }\\n  __typename\\n}\\n\"}"))
                .Deserialize<ReviewsResponse>();
            return reviewResponse;
        }

        public async Task<(string thanks, string apology)> GetTemplateId(string reviewId)
        {
            Notifier.Display("Getting messages Templates");
            var json = await PostJson("https://express.medallia.com/api-comp/reporting/graphql", "[{\"operationName\":\"getRapidResponseDropdown\",\"variables\":{\"dataView\":{\"id\":\"52\",\"minimumSampleSizeEnabled\":false},\"responseId\":\"" + reviewId + "\"},\"query\":\"query getRapidResponseDropdown($dataView: DataView, $responseId: ID!) {\\n  rapidResponseTemplates(dataView: $dataView, requestAllPages: true, responseId: $responseId) {\\n    items {\\n      id\\n      name\\n      draft {\\n        lastSaved\\n        __typename\\n      }\\n      __typename\\n    }\\n    permissions {\\n      canReadRapidResponseTemplates\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"},{\"operationName\":\"getEmailSettings\",\"variables\":{},\"query\":\"query getEmailSettings {\\n  rapidResponseSettings {\\n    name\\n    title\\n    affiliation\\n    emailAddress\\n    phone\\n    permissions {\\n      canUpdateRapidResponseSettings\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}]");
            var array = JArray.Parse(json);
            var items = array.First?.SelectToken("data.rapidResponseTemplates.items");
            if (items == null) throw new KnownException($"Null template items");
            var thanks = "";
            var apology = "";
            foreach (var item in items)
            {
                var id = (string)item.SelectToken("id");
                var name = (string)item.SelectToken("name");
                switch (name)
                {
                    case "English - Thank You":
                        thanks = id;
                        break;
                    case "English - Apology ":
                        apology = id;
                        break;
                }
            }

            return (thanks, apology);
        }

        public async Task<RapidResponseTemplateResponse> GetMessage(string reviewId, string templateId)
        {
            Notifier.Display("Preparing response");
            var json = await PostJson("https://express.medallia.com/api-comp/reporting/graphql", "[{\"operationName\":\"rapidResponseEditorQuery\",\"variables\":{\"isDraft\":false,\"dataView\":{\"id\":\"52\",\"minimumSampleSizeEnabled\":false},\"responseId\":\"" + reviewId + "\",\"templateId\":\"" + templateId + "\"},\"query\":\"query rapidResponseEditorQuery($dataView: DataView, $isDraft: Boolean = false, $responseId: ID!, $templateId: ID!) {\\n  rapidResponseTemplate(dataView: $dataView, id: $templateId, responseId: $responseId) {\\n    id\\n    name\\n    emailSubject\\n    emailBody\\n    emailTo\\n    emailCcEnabled\\n    emailCc\\n    emailCcFixed\\n    supportsMarkup\\n    draft @include(if: $isDraft) {\\n      emailSubject\\n      emailBody\\n      emailCc\\n      lastSaved\\n      __typename\\n    }\\n    __typename\\n  }\\n}\\n\"}]");
            var arr = JArray.Parse(json);
            if (arr.First == null) throw new KnownException($"null array for rapid response message");
            var message = JsonConvert.DeserializeObject<RapidResponseTemplateResponse>(arr.First.ToString());
            return message;
        }

        public async Task Reply(string reviewId, string templateId, RapidResponseTemplateResponse t)
        {
            Notifier.Display($"Sending replay to {t.data.rapidResponseTemplate.emailTo}");
            var cc = t.data.rapidResponseTemplate.emailCc.First().GetStringBetween("<", ">");
            var req = new ResponseReq { variables = { responseId = reviewId, templateId = templateId } };
            req.variables.message.emailCc.Add(cc);
            req.variables.message.emailBody = t.data.rapidResponseTemplate.emailBody;
            req.variables.message.emailSubject = t.data.rapidResponseTemplate.emailSubject;
            var res = JsonConvert.SerializeObject(new List<ResponseReq>() { req });
            var s = await PostJson("https://express.medallia.com/api-comp/reporting/graphql", res);
            if (!s.Contains("statusCode\":200"))
                throw new Exception($"Error responding to {reviewId}, template Id={templateId}: \n {s}");
        }

        public async Task CloseAlert(string alertId)
        {
            var s = await PostJson("https://express.medallia.com/api-comp/reporting/graphql", "[{\"operationName\":\"changeAlertStatus\",\"variables\":{\"alertId\":\"" + alertId + "\",\"note\":\"\",\"alertStatusId\":\"CLOSED\",\"dataView\":{\"id\":\"52\",\"minimumSampleSizeEnabled\":false}},\"query\":\"mutation changeAlertStatus($alertId: ID!, $note: String, $alertStatusId: ID!, $dataView: DataView!) {\\n  updateAlertStatusWeb(alertId: $alertId, note: $note, alertStatusId: $alertStatusId, dataView: $dataView, throwError: true) {\\n    id\\n    label\\n    __typename\\n  }\\n}\\n\"}]");
        }

        public async Task CloseOnSocial(string reviewId)
        {
            var s = await PostJson("https://express.medallia.com/api-comp/reporting/graphql", "[{\"operationName\":\"sendExternalReply\",\"variables\":{\"responseId\":\"" + reviewId + "\"},\"query\":\"mutation sendExternalReply($responseId: ID!) {\\n  publishSocialFeedbackReply(responseId: $responseId, responseReply: {externalReply: true}) {\\n    status\\n    __typename\\n  }\\n}\\n\"}]");
        }

        private async Task<string> PostJson(string url, string json)
        {
            return await _httpClient.PostJson(url, json, 1, new Dictionary<string, string>
            {
                { "x-csrf-token", _csrf }
            }, _ct);
        }
    }
}