using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventWebApi.Client;
using EventWebApi.Client.Models;
using EventWebApi.PactConsumer.Tests.MockPactProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet.Mocks.MockHttpService.Models;

namespace EventWebApi.PactConsumer.Tests.PactTests
{
    [TestClass]
    public class StatsApiConsumerPactTests
    {

        private static ConsumerStatsApiPact ConsumerStatsApiPact { get; set; }


        protected readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly Dictionary<string, string> Headers = new Dictionary<string, string> { { "Content-Type", "application/json; charset=utf-8" } };

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ConsumerStatsApiPact = new ConsumerStatsApiPact();
            
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ConsumerStatsApiPact.Service.ClearInteractions();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ConsumerStatsApiPact.Build();
        }
        

        [TestMethod]
        public void StatsApi_IsAlive_Valid()
        {
            
            Setup_IsAlive();


            var client = new StatsApiClient(ConsumerStatsApiPact.BaseUri);
            bool response = client.IsAlive();

            Assert.IsTrue(response);

            ConsumerStatsApiPact.Service.VerifyInteractions();


        }

        private void Setup_IsAlive()
        {
            var mockResponse = new
            {
                alive = true,
                _links = new Dictionary<string, dynamic> {{"uptime", new {href = "/stats/uptime"}}}
            };

            
            ConsumerStatsApiPact.Service.UponReceiving("a valid request for status")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/stats/status"
                }).WillRespondWith(new ProviderServiceResponse {Body = mockResponse, Status = 200, Headers = Headers});
        }

        [TestMethod]
        public void StatsApi_Uptime_Valid()
        {

            Setup_IsAlive();

            var upSinceDate = new DateTime(2001, 1, 1);
            var mockResponse = new {};

            ConsumerStatsApiPact.Service.UponReceiving("a valid request for uptime")
                .With(new ProviderServiceRequest {Method = HttpVerb.Get, Path = "/stats/uptime"})
                .WillRespondWith(new ProviderServiceResponse {Status = 200, Headers=Headers,  Body = new {upSince = upSinceDate}});

            var client = new StatsApiClient(ConsumerStatsApiPact.BaseUri);

            var date = client.UpSince();

            Assert.AreEqual(upSinceDate, date);

            ConsumerStatsApiPact.Service.VerifyInteractions();
        }
    }
}
