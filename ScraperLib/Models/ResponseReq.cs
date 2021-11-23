using System.Collections.Generic;

namespace BestWesternBot.Models
{
    public class ResponseReq
    {
        public string operationName { get; set; } = "submitRapidResponse";
        public Variables variables { get; set; }=new Variables();
        public string query { get; set; } = "mutation submitRapidResponse($dataView: DataView, $responseId: ID!, $templateId: ID!, $message: RapidResponseMessageInput!) {\n  RapidResponseMessageSend(dataView: $dataView, responseId: $responseId, templateId: $templateId, message: $message) {\n    __typename\n    ... on RapidResponseData {\n      statusCode\n      templateId\n      __typename\n    }\n    ... on InputValidationError {\n      errorType\n      validationError\n      perPropertyErrorMessages {\n        errorType\n        field\n        message\n        value\n        __typename\n      }\n      __typename\n    }\n  }\n}\n";


        public class Variables
        {
            public Dataview dataView { get; set; }=new Dataview();
            public string responseId { get; set; }
            public string templateId { get; set; }
            public Message message { get; set; }=new Message();
        }

        public class Dataview
        {
            public string id { get; set; } = "52";
            public bool minimumSampleSizeEnabled { get; set; } = false;
        }

        public class Message
        {
            public List<string> emailCc { get; set; } = new List<string>();
            public string emailBody { get; set; }
            public string emailSubject { get; set; }
            public List<string> files { get; set; } = new List<string>();
        }

    }
}