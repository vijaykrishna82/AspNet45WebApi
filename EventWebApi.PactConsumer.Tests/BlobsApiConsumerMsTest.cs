using EventWebApi.Client;
using EventWebApi.PactConsumer.Tests.MockPactProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet.Mocks.MockHttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace EventWebApi.PactConsumer.Tests
{
    /// <summary>
    ///     Summary description for BlobsApiConsumerMsTest
    /// </summary>
    [TestClass]
    public class BlobsApiConsumerMsTest
    {
        private static ConsumerBlobApiPact ConsumerBlobApiPact;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            ConsumerBlobApiPact = new ConsumerBlobApiPact();
            ConsumerBlobApiPact.Service.ClearInteractions();
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            //generate the json at the end of test run.
            ConsumerBlobApiPact.Build();
        }

        [TestMethod]
        public void BlobApi_CreateBlob_Valid()
        {
            //Arrange
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            var bytes = Encoding.UTF8.GetBytes("This is a test");

            ConsumerBlobApiPact.Service.UponReceiving("a valid request to create a new blob")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = $"/blobs/{blobId}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/octet-stream" } },
                    Body = bytes
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 201 });

            var consumer = new BlobsApiClient(ConsumerBlobApiPact.BaseUri);

            //Act / Assert
            consumer.CreateBlob(blobId, bytes, "test.txt");

            ConsumerBlobApiPact.Service.VerifyInteractions();
        }

        [TestMethod]
        public void BlobApi_CreateBlob_BadRequest()
        {
            //Arrange
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            var bytes = Encoding.UTF8.GetBytes("This is a test and something");

            ConsumerBlobApiPact.Service.UponReceiving("a bad request to create a new blob")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = $"/blobs/{blobId}",
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/octet-stream" } },
                    Body = bytes
                })
                .WillRespondWith(new ProviderServiceResponse { Status = 400 });

            var consumer = new BlobsApiClient(ConsumerBlobApiPact.BaseUri);

            //Act / Assert
            try
            {
                consumer.CreateBlob(blobId, bytes, "test.txt");
            }
            catch (HttpRequestException ex)
            {
                Assert.AreEqual("The Events API request for POST http://localhost:1235/blobs/38c3976b-5ae8-4f2f-a8ec-46f6aee826e2 failed. Response Status: 400, Response Body: ", ex.Message);
            }

            ConsumerBlobApiPact.Service.VerifyInteractions();
        }

        [TestMethod]
        public void BlobApi_GetBlob_Valid()
        {
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            ConsumerBlobApiPact.Service.UponReceiving("a valid request to get blob")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = $"/blobs/{blobId}"
                }).WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Body = "This is a test",
                    Headers = new Dictionary<string, string> {{"Content-Type", "text/plain"}}
                });

            var consumer = new BlobsApiClient(ConsumerBlobApiPact.BaseUri);

            var bytes = Encoding.UTF8.GetBytes("This is a test");
            var response = consumer.GetBlob(blobId);

            Assert.IsTrue(bytes.SequenceEqual(response));

            ConsumerBlobApiPact.Service.VerifyInteractions();
        }  
    }
}