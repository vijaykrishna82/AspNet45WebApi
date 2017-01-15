using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace EventWebApi.PactConsumer.Tests.MockPactProviders
{
    public class PactBase
    {

        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService Service { get; private set; }

        public int Port { get; private set; }
        public string BaseUri => $"http://localhost:{Port}";

        public PactBase(string consumer, string provider, int port)
        {
            PactBuilder = new PactBuilder()
                .ServiceConsumer(consumer)
                .HasPactWith(provider);
            Port = port;
            Service = PactBuilder.MockService(Port);
        }

        public void Build()
        {
            PactBuilder.Build();
        }
    }
}
