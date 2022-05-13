using System;
using Amazon.DynamoDBv2.DataModel;

namespace SafeNote.WebAPI.Controllers
{
    [DynamoDBTable("code-2022-05-13")]
    public class CodeNote
    {
        [DynamoDBHashKey("id")] 
        public string Id { get; set; }

        public string Data { get; set; }
        public string Salt { get; set; }
        public DateTime Updated { get; set; }
    }
}