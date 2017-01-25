using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HowAngryTwitter.Services
{
    public class NetworkServices
    {
        // GET a la API de Twitter
        private string TwitterAPIgetTimeline = "https://api.twitter.com/1.1/statuses/user_timeline.json?count=200&screen_name={0}&exclude_replies=true&trim_user=true&include_rts=false"; // {0} -> El username de Twitter

        // POST a la API de Microsoft cognitives Services
        private string MCSemotionalAPI = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        private string MCSlanguageAPI = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/languages";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.TwitterTimeline>> GetTimelineTwitter(string username)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAAAszywAAAAAAUWBaRLJByJ6Gmflr%2FMQE5%2FRJaCM%3DwoGpMOLIby5siXnKRM991CyUs4zCZn7nYJJChTXNEkrI5B2WQK");
                //client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip")); // Con esto falla aquí, no en el POSTMAN

                var json = await client.GetStringAsync(string.Format(TwitterAPIgetTimeline, username));

                List<Models.TwitterTimeline> TwitterTimeline = JsonConvert.DeserializeObject<List<Models.TwitterTimeline>>(json);

                return TwitterTimeline;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.LanguageDocument>> PostTimelineLanguages(List<Models.TwitterTimeline> TwitterTimeline, string Body)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0cda1251b3704bdeb52dbd286b9c9164");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent theContent = new StringContent(Body, Encoding.UTF8, "application/json");

                var response = client.PostAsync(MCSlanguageAPI, theContent).GetAwaiter().GetResult();
                var json = await response.Content.ReadAsStringAsync();

                Models.MCSLanguagesResponse LanguagesTimeline = JsonConvert.DeserializeObject<Models.MCSLanguagesResponse>(json);

                return LanguagesTimeline.documents;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Models.SentimentDocument>> PostTimelineEmotions(List<Models.TwitterTimeline> TwitterTimeline, string Body)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0cda1251b3704bdeb52dbd286b9c9164");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                //client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent theContent = new StringContent(Body, Encoding.UTF8, "application/json");

                var response = client.PostAsync(MCSemotionalAPI, theContent).GetAwaiter().GetResult();
                var json = await response.Content.ReadAsStringAsync();

                Models.MCSSentimentResponse SentimentalTimeline = JsonConvert.DeserializeObject<Models.MCSSentimentResponse>(json);

                return SentimentalTimeline.documents;
            }
        }
    }
}
