using System;
using System.Net.Http;
using EventWebApi.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet;
using PactNet.Mocks.MockHttpService.Models;

namespace EventWebApi.PactProvider.Tests
{
    [TestClass]
    public class EventApiPactVerifierTests
    {
        [TestMethod]
        public void EnsureEventApiHonoursPactWithConsumer()
        {
            IPactVerifier pactVerifier = new PactVerifier(() => { }, () => { });


            pactVerifier.ProviderState("interactions exist", () => { });

            using (var client = new HttpClient() {BaseAddress = new Uri("http://localhost:50807")})
            {
                pactVerifier.ServiceProvider("Event Api", client)
                    .HonoursPactWith("Consumer")
                    .PactUri(@"..\..\..\EventWebApi.PactConsumer.Tests\pacts\consumer-event_api.json")
                    //D:\Learning\PACT\AspNet45WebApi\
                    .Verify();
            }

            //pactVerifier.ServiceProvider("Event Api", (ProviderServiceRequest request) =>
            //{
            //    switch (request.Path)
            //    {
            //        case "/events":
            //            var response = 
            //    }

            //    return new ProviderServiceResponse
            //    {

            //    };
            //});
        }
    }
}
