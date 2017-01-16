using System;
using EventWebApi.PactProvider.Tests.MockServerInfrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin.Testing;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.OAuth;
using PactNet;

namespace EventWebApi.PactProvider.Tests
{
    [TestClass]
    public class ConsumerEventApiPactTests
    {
        private TestServer TestServer { get; set; }

        [TestMethod]
        public void EventApiHonoursPactWithConsumer()
        {
            var outputter = new CustomOutputter();

            var config = new PactVerifierConfig();
            config.ReportOutputters.Add(outputter);

            IPactVerifier verifier = new PactVerifier(setUp: () => { }, tearDown: () => { }, config: config);

            verifier.ProviderState("interactions exist", setUp: AddInteractionsToDatabase);

            TestServer = TestServer.Create(appBuilder =>
            {
                appBuilder.Use(typeof (AuthorizationTokenReplacementMiddleware), 
                    appBuilder.CreateDataProtector(typeof (OAuthAuthorizationServerMiddleware).Namespace, "Access_Token", "v1"));
                var apiStartup = new OwinStartup();
                apiStartup.Configuration(appBuilder);
            });

            verifier.ServiceProvider("Event Api", TestServer.HttpClient).HonoursPactWith("Consumer")
                .PactUri("../../../pacts/consumer-event_api.json")
                .Verify();

            Assert.IsTrue(outputter.Output.Contains("Verifying a Pact between Consumer and Event API"));
        }

        private void AddInteractionsToDatabase()
        {
            

        }
    }
}
