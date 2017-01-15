using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace EventWebApi.PactConsumer.Tests.MockPactProviders
{
    public class ConsumerBlobApiPact  : PactBase
    {
        public ConsumerBlobApiPact() : base("Consumer", "Blob Api", 1235)
        {
        }
    }
}