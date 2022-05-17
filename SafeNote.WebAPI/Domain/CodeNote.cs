using System;
using Amazon.DynamoDBv2.DataModel;

namespace SafeNote.WebAPI.Controllers
{
    [DynamoDBTable("safenote-2022-05-04")]
    public class CodeNote
    {
        [DynamoDBHashKey] 
        public string Id { get; set; }

        public string Data { get; set; }
        public string Salt { get; set; }
        public DateTime Updated { get; set; }
    }
}