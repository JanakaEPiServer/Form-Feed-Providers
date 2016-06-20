using EPiServer.Forms.Core.ExternalFeed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FormFeeds.Json.Core
{
    public class KeyValueJsonFeedProvider : JsonFeedProvider
    {
       
        private const char PipeSeperator = '|';

        internal override List<IFeedItem> LoadListFromJson(string json, string configuration)
        {
            List<IFeedItem> list = new List<IFeedItem>();
            string[] properties = configuration.Split(new char[] { PipeSeperator });
            bool feedItemFinished = false;
            FeedItem feedItem = new FeedItem();

            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {                

                if (reader.Value != null && reader.TokenType == JsonToken.PropertyName)
                {
                    string propName = reader.Value.ToString();
                    if (properties.Contains(propName))
                    {
                        bool isKey = properties[0] == propName;
                        string propertyValue = reader.ReadAsString();
                        if (isKey)
                        {
                            feedItem.Key = propertyValue;
                        }
                        else
                        {
                            feedItem.Value = propertyValue;
                        }

                        feedItemFinished = (!string.IsNullOrEmpty(feedItem.Key) && !string.IsNullOrEmpty(feedItem.Value));
                    }
                }

                if (feedItemFinished)
                {
                    list.Add(feedItem);
                    feedItem = new FeedItem();
                    feedItemFinished = false;
                }
            }

            return list;
        }

        
    }
}
