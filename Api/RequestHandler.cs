using StoryMaker.Models.Api;
using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace StoryMaker.Api
{
   public class RequestHandler
    {

        public static async Task<StaticPagesResponse> getTerms() {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync("static-pages/aboutus")) {
                if (response.IsSuccessStatusCode)
                {
                    StaticPagesResponse staticPages = await response.Content.ReadAsAsync<StaticPagesResponse>();
                    return staticPages;
                }
                else {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
