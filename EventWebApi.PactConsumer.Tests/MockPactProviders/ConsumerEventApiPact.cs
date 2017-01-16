using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace EventWebApi.PactConsumer.Tests.MockPactProviders
{
    public class ConsumerEventApiPact : PactBase
    {
        
        public ConsumerEventApiPact() : base("Consumer", "Event API", 1234)
        {
            
        }
        
    }
}