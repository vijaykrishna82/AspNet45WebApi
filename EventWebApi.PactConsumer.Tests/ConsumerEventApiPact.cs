using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace EventWebApi.PactConsumer.Tests
{
    public class ConsumerEventApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 1234;
        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerEventApiPact()
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer("Consumer")
                .HasPactWith("Event API");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}