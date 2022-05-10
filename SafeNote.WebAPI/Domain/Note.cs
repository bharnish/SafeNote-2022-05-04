using System;
using Amazon.DynamoDBv2.DataModel;

namespace SafeNote.WebAPI.Domain
{
    [DynamoDBTable("safenote-2022-05-04")]
    public class Note
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Data { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}