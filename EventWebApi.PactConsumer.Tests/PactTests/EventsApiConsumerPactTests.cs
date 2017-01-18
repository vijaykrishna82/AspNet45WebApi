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


        private readonly Dictionary<string,string> AcceptHeaders = 
            new Dictionary<string, string> { { "Accept", "application/json"} };
        private readonly Dictionary<string, string> ContentTypeCharset =
            new Dictionary<string, string> { { "Content-Type", "application/json; charset=utf-8" } };

        private readonly Dictionary<string, string> ContentTypeHeaders =
            new Dictionary<string, string> { { "Content-Type", "application/json; charset=utf-8" } };

        [TestMethod]
        public void EventApi_GetEvents_NoAuthorization()
        {
            ConsumerEventApiPact.Service.Given("interactions exist")
                .UponReceiving("a valid request without authorization for GetEvents")
                .With(new ProviderServiceRequest {Method = HttpVerb.Get, Path = "/events", Headers = AcceptHeaders})
                .WillRespondWith(new ProviderServiceResponse
                {
                    Headers = ContentTypeHeaders,
                    Status = 401,
                    Body = new {Message = "Authorization has been denied for this request."}
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
            var requestHeaders = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                {"Authorization", $"Bearer {authToken}" }
            };

            ConsumerEventApiPact.Service.Given("interactions exist")
                .UponReceiving("a valid request for GetEvents")
                .With(new ProviderServiceRequest {Method = HttpVerb.Get, Path = "/events", Headers = requestHeaders})
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = ContentTypeHeaders,
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

            ConsumerEventApiPact.Service.VerifyInteractions();
        }

        [TestMethod]
        public void EventApi_CreateEvent_Valid()
        {
            var eventId = Guid.Parse("1F587704-2DCC-4313-A233-7B62B4B469DB");
            var dateTime = new DateTime(2011, 07, 01, 01, 41, 03);

            DateTimeFactory.Now = () => dateTime;

            ConsumerEventApiPact.Service
                .UponReceiving("a valid request for CreateEvent")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/events",
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        eventId,
                        timestamp = dateTime.ToString("O"),
                        eventType = "DetailsView"
                    }
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 201 });

            var client = new EventsApiClient(ConsumerEventApiPact.BaseUri);

            client.CreateEvent(eventId);

            ConsumerEventApiPact.Service.VerifyInteractions();
                
        }
    }
}
