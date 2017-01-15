using System;
using System.Text;
using System.Collections.Generic;
using EventWebApi.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventWebApi.ComponentTests
{
    /// <summary>
    /// Summary description for BlobClientTests
    /// </summary>
    [TestClass]
    public class BlobClientTests
    {
        [TestMethod]
        public void Blobs_CreateBlob_DeploymentTest()
        {
            var client = new BlobsApiClient("http://localhost/EventWebApi");

            var bytes = Encoding.UTF8.GetBytes("This is a test");
            client.CreateBlob(Guid.NewGuid(), bytes, "test");


        }

        [TestMethod]
        public void Blobs_GetBlob_DeploymentTest()
        {
            var client = new BlobsApiClient("http://localhost/EventWebApi");

           byte[] bytes=  client.GetBlob(Guid.NewGuid());
            var text = Encoding.UTF8.GetString(bytes);

            Assert.AreEqual("This is a test", text);
        }
    }
}
