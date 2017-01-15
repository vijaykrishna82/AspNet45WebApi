using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventWebApi.Client;

namespace EventWebApi.ComponentTests
{
    [TestClass]
    public class StatsClientTests
    {
        [TestMethod]
        public void Upsince_DeploymentTest()
        {
            StatsApiClient client = new StatsApiClient("http://localhost/EventWebApi");

            DateTime? date = client.UpSince();

            Assert.IsNotNull(date);
        }

        [TestMethod]
        public void IsAlive_DeploymentTest()
        {
            var client = new StatsApiClient("http://localhost/EventWebApi");
            bool isAlive = client.IsAlive();
            Assert.IsTrue(isAlive);
        }
    }
}
