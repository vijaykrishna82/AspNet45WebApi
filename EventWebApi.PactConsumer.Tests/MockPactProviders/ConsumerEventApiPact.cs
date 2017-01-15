using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace EventWebApi.PactConsumer.Tests.MockPactProviders
{
    public class ConsumerEventApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get; private set; }
        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerEventApiPact()
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer("Consumer")
                .HasPactWith("Event API");
            MockServerPort = 1234;
            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}