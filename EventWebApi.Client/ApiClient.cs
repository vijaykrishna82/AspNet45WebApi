using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventWebApi.Client
{
    public class ApiClient : IDisposable
    {
        protected readonly HttpClient HttpClient;

        protected string BaseUri { get; set; }

        protected readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public ApiClient(string baseUri = null, string authToken = null)
        {
            HttpClient = new HttpClient();
            BaseUri = baseUri;

            if (authToken != null)
            {
                HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            }
        }

        public void Dispose()
        {
            Dispose(HttpClient);
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
                string.Format("The Events API request for {0} {1} failed. Response Status: {2}, Response Body: {3}",
                    failedRequest.Method.ToString().ToUpperInvariant(),
                    failedRequest.RequestUri,
                    (int) failedResponse.StatusCode,
                    failedResponse.Content.ReadAsStringAsync().Result));
        }
    }
}