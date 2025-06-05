using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace StoryMaker.Api
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }
        public static void initClient() {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://1teen.vas24.ir/");
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ApiClient.DefaultRequestHeaders.Add("App","**********");
            ApiClient.DefaultRequestHeaders.Add("User-Token", "k******************************************");
            ApiClient.DefaultRequestHeaders.Add("Device-Id", "****************************");
            ApiClient.DefaultRequestHeaders.Add("Mobile", "*******************************");

        }
    }
}
