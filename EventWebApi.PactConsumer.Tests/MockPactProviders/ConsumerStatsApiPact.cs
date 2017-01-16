using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApi.PactConsumer.Tests.MockPactProviders
{
    public class ConsumerStatsApiPact : PactBase
    {
        public ConsumerStatsApiPact() : base("Consumer", "Stats Api", 1236)
        {
        }
    }
}
