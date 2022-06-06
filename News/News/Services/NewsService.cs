using News.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace News.Services
{
    public class NewsService
    {

        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "d7ee1b401fe34f1ab46845840212f712";

        public event EventHandler<string> NewsAvailable;

        protected virtual void OnNewsAvailable(string e)
        {
            NewsAvailable?.Invoke(this, e);
        }

        public async Task<NewsGroup> GetNewsAsync(NewsCategory category)
        {

#if UseNewsApiSample
            NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);


#else
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

            /* HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>(); */

            NewsGroup newsgroup = new();

            var date = DateTime.UtcNow;

            NewsCacheKey cachekey = new(category, date);
            if (!cachekey.CacheExist)
            {
                var webclient = new WebClient();
                var json = await webclient.DownloadStringTaskAsync(uri);
                NewsApiData nd = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsApiData>(json);


                newsgroup.Category = category;
                newsgroup.Articles = nd.Articles.Select(item => new NewsItem
                {
                    DateTime = item.PublishedAt,
                    Title = item.Title,
                    Description = item.Description,
                    Url = item.Url,
                    UrlToImage = item.UrlToImage
                }).ToList();

                NewsGroup.Serialize(newsgroup, "test.xml");
                NewsCacheKey.Serialize(newsgroup, cachekey.FileName);
                OnNewsAvailable($"News in category is available: {category}");

            }
            else
            {
                newsgroup = NewsCacheKey.Deserialize(cachekey.FileName);
                OnNewsAvailable($"XML Cached news in category is available: {category}");
            }

#endif
            return newsgroup;
        }
    }


}

