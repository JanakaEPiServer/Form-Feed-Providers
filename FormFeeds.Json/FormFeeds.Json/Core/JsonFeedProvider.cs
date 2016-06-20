using EPiServer.Forms.Core.ExternalFeed;
using EPiServer.Logging;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FormFeeds.Json.Core
{
    public abstract class JsonFeedProvider : IFeedProvider
    {
        protected static readonly ILogger _log = LogManager.GetLogger();
        

        public virtual IEnumerable<IFeedItem> LoadItems(IFeed feedToLoad)
        {
            List<IFeedItem> list = new List<IFeedItem>();            

            if (ValidateFeed(feedToLoad))
            {
                string json = RetrieveJsonFromFeed(feedToLoad);
                list = LoadListFromJson(json, feedToLoad.ExtraConfiguration);
            }

            return list;            
        }

        internal abstract List<IFeedItem> LoadListFromJson(string json, string configuration);

        protected virtual bool ValidateFeed(IFeed feedToLoad)
        {
            if (string.IsNullOrEmpty(feedToLoad.Url) || string.IsNullOrEmpty(feedToLoad.ExtraConfiguration))
            {
                LoggerExtensions.Error(_log, "Wrong settings for loading feed ID: {0}", feedToLoad.ID);
                return false;
            }

            return true;
        }

        protected string RetrieveJsonFromFeed(IFeed feedToLoad)
        {
            
            string jsonContents = string.Empty;
            HttpResponseMessage result = new HttpClient()
            {
                DefaultRequestHeaders = {
                  Accept = {
                    new MediaTypeWithQualityHeaderValue("application/json")
                  }
            }
            }.GetAsync(feedToLoad.Url).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                jsonContents = result.Content.ReadAsStringAsync().Result;
            }
            else
            {
                LoggerExtensions.Error(_log, "Cannot load feed items for feed ID: {0} - URL: {1}. The response return with code {2}", feedToLoad.ID, feedToLoad.Url, result.StatusCode);
            }

            return jsonContents;
        }
    }
}
