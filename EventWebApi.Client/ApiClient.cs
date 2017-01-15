using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApi.Client
{
    public class ApiClient : IDisposable
    {
        protected readonly HttpClient _httpClient;

        public ApiClient(string baseUri = null, string authToken = null)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://my.api/v2/capture") };

            if (authToken != null)
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            }
        }

        protected readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public void Dispose()
        {
            Dispose(_httpClient);
        }

        public void Dispose(params IDisposable[] disposables)
        {
            foreach (var disposable in disposables.Where(d => d != null))
            {
                disposable.Dispose();
            }
        }


        protected static void RaiseResponseError(HttpRequestMessage failedRequest, HttpResponseMessage failedResponse)
        {
            throw new HttpRequestException(
                String.Format("The Events API request for {0} {1} failed. Response Status: {2}, Response Body: {3}",
                failedRequest.Method.ToString().ToUpperInvariant(),
                failedRequest.RequestUri,
                (int)failedResponse.StatusCode,
                failedResponse.Content.ReadAsStringAsync().Result));
        }
    }
}
