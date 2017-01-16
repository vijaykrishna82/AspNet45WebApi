using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using EventWebApi.Client;
using EventWebApi.PactConsumer.Tests.MockPactProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet.Mocks.MockHttpService.Models;

namespace EventWebApi.PactConsumer.Tests.PactTests
{
    [TestClass]
    public class EventsApiConsumerPactTests
    {
        private static ConsumerEventApiPact ConsumerEventApiPact { get;  set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ConsumerEventApiPact = new ConsumerEventApiPact();

        }

        [TestInitialize]
        public void TestInitialize()
        {
            ConsumerEventApiPact.Service.ClearInteractions();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ConsumerEventApiPact.Build();
        }


        private readonly Dictionary<string,string> RequestHeaders = 
            new Dictionary<string, string> { { "Accept", "application/json"} };
        private readonly Dictionary<string, string> ResponseHeaders =
            new Dictionary<string, string> { { "Content-Type", "application/json" } };

        [TestMethod]
        public void EventApi_GetEvents_NoAuthorization()
        {
            ConsumerEventApiPact.Service.Given("interactions exist")
                .UponReceiving("a valid request without authorization for GetEvents")
                .With(new ProviderServiceRequest {Method = HttpVerb.Get, Path = "/events", Headers = RequestHeaders})
                .WillRespondWith(new ProviderServiceResponse
                {
                    Headers = ResponseHeaders,
                    Status = 401,
                    Body = new {message = "Authorization has been denied for this request"}
                });

            var client = new EventsApiClient(ConsumerEventApiPact.BaseUri);

            try
            {
                var response = client.GetAllEvents();
            }
            catch (HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Authorization has been denied for this request"));
            }

            ConsumerEventApiPact.Service.VerifyInteractions();
        }

        [TestMethod]
        public void EventApi_GetAllEvents_Valid()
        {
            var authToken = "someValidAuthToken";
            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" },
                {"Authorization", $"Bearer {authToken}" }
            };

            ConsumerEventApiPact.Service.Given("interactions exist")
                .UponReceiving("a valid request for GetEvents")
                .With(new ProviderServiceRequest {Method = HttpVerb.Get, Path = "/events", Headers = headers})
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = ResponseHeaders,
                    Body = new[]
                    {
                        new
                        {
                            eventId = Guid.Parse("45D80D13-D5A2-48D7-8353-CBB4C0EAABF5"),
                            timestamp = "2014-06-30T01:37:41.0660548",
                            eventType = "SearchView"
                        },
                        new
                        {
                            eventId = Guid.Parse("83F9262F-28F1-4703-AB1A-8CFD9E8249C9"),
                            timestamp = "2014-06-30T01:37:52.2618864",
                            eventType = "DetailsView"
                        },
                        new
                        {
                            eventId = Guid.Parse("3E83A96B-2A0C-49B1-9959-26DF23F83AEB"),
                            timestamp = "2014-06-30T01:38:00.8518952",
                            eventType = "SearchView"
                        }
                    }
                });

            var client = new EventsApiClient(ConsumerEventApiPact.BaseUri, authToken);
            var events = client.GetAllEvents();

            Assert.AreEqual(3, events.Count());

        }
    }
}
