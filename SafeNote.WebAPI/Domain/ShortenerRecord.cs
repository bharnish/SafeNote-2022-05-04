using System;
using Amazon.DynamoDBv2.DataModel;

namespace SafeNote.WebAPI.Domain
{
    [DynamoDBTable("urlshortener-2022-05-11")]
    public class ShortenerRecord
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }

        public string Data { get; set; }

        public string Salt { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}