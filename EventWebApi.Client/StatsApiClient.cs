using EventWebApi.Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApi.Client
{
    public class StatsApiClient : ApiClient
    {
        public StatsApiClient(string baseUri = null, string authToken = null) : base(baseUri, authToken) { }

        public bool IsAlive()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/stats/status");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = JsonConvert.DeserializeObject<dynamic>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                    return responseContent.alive;
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return false;
        }

        public DateTime? UpSince()
        {
            var statusRequest = new HttpRequestMessage(HttpMethod.Get, "/stats/status");
            statusRequest.Headers.Add("Accept", "application/json");

            var statusResponse = _httpClient.SendAsync(statusRequest);

            try
            {
                var statusResult = statusResponse.Result;
                var statusResponseBody = new StatusResponseBody();

                if (statusResult.StatusCode == HttpStatusCode.OK)
                {
                    statusResponseBody = JsonConvert.DeserializeObject<StatusResponseBody>(statusResult.Content.ReadAsStringAsync().Result, _jsonSettings);
                }
                else
                {
                    RaiseResponseError(statusRequest, statusResult);
                }

                if (statusResponseBody.Alive)
                {
                    var uptimeLink = statusResponseBody.Links.Single(x => x.Key.Equals("uptime")).Value.Href;

                    if (!String.IsNullOrEmpty(uptimeLink))
                    {
                        var uptimeRequest = new HttpRequestMessage(HttpMethod.Get, uptimeLink);
                        uptimeRequest.Headers.Add("Accept", "application/json");

                        var uptimeResponse = _httpClient.SendAsync(uptimeRequest);
                        try
                        {
                            var uptimeResult = uptimeResponse.Result;
                            if (uptimeResult.StatusCode == HttpStatusCode.OK)
                            {
                                var uptimeResponseBody = JsonConvert.DeserializeObject<UptimeResponseBody>(uptimeResult.Content.ReadAsStringAsync().Result, _jsonSettings);
                                return uptimeResponseBody.UpSince;
                            }

                            RaiseResponseError(uptimeRequest, uptimeResult);
                        }
                        finally
                        {
                            Dispose(uptimeRequest, uptimeResponse);
                        }
                    }
                }
            }
            finally
            {
                Dispose(statusRequest, statusResponse);
            }

            return null;
        }
    }
}
