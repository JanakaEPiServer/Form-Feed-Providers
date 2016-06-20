using EPiServer.Forms.Core.ExternalFeed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace FormFeeds.Json.Core
{
    public class SimpleJsonFeedProvider : JsonFeedProvider
    {
        internal override List<IFeedItem> LoadListFromJson(string json, string configuration)
        {
            List<IFeedItem> list = new List<IFeedItem>();
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if (reader.Value != null && reader.TokenType == JsonToken.PropertyName)
                {
                    if (reader.Value.ToString() == configuration)
                    {
                        string propertyValue = reader.ReadAsString();
                        list.Add(new FeedItem() { Key = propertyValue, Value = propertyValue });
                    }
                }
            }
            return list;
        }
    }
}
