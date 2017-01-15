using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventWebApi.Client;

namespace EventWebApi.ComponentTests
{
    [TestClass]
    public class StatsClientTests
    {
        [TestMethod]
        public void Stats_Upsince_DeploymentTest()
        {
            var client = new StatsApiClient("http://localhost/EventWebApi");

            var date = client.UpSince();

            Assert.IsNotNull(date);
        }

        [TestMethod]
        public void Stats_IsAlive_DeploymentTest()
        {
            var client = new StatsApiClient("http://localhost/EventWebApi");
            bool isAlive = client.IsAlive();
            Assert.IsTrue(isAlive);
        }
    }
}
