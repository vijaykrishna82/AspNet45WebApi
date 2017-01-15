using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using EventWebApi.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventWebApi.Client
{
    public class EventsApiClient : ApiClient
    {

        public EventsApiClient(string baseUri = null, string authToken = null) : base(baseUri, authToken) { }

        public IEnumerable<Event> GetAllEvents()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/events");
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    return !String.IsNullOrEmpty(content)
                                ? JsonConvert.DeserializeObject<IEnumerable<Event>>(content, _jsonSettings)
                                : new List<Event>();
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        public Event GetEventById(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format("/events/{0}", id));
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<Event>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        public IEnumerable<Event> GetEventsByType(string eventType)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, String.Format("/events?type={0}", eventType));
            request.Headers.Add("Accept", "application/json");

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<Event>>(result.Content.ReadAsStringAsync().Result, _jsonSettings);
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }

        public void CreateEvent(Guid eventId, string eventType = "DetailsView")
        {
            var @event = new
            {
                EventId = eventId,
                Timestamp = DateTimeFactory.Now().ToString("O"),
                EventType = eventType
            };

            var eventJson = JsonConvert.SerializeObject(@event, _jsonSettings);
            var requestContent = new StringContent(eventJson, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/events") { Content = requestContent };

            var response = _httpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                var statusCode = result.StatusCode;
                if (statusCode == HttpStatusCode.Created)
                {
                    return;
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }
        }

    }
}
