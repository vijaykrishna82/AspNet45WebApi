using System;
using System.Collections.Generic;
using System.Text;
using EventWebApi.Client;
using EventWebApi.PactConsumer.Tests.MockPactProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;

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
            ConsumerBlobApiPact.Build();
        }

        [TestMethod]
        public void BlobApi_CreateBlob_Test()
        {
            //Arrange
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            var bytes = Encoding.UTF8.GetBytes("This is a test");

            ConsumerBlobApiPact.Service.UponReceiving("a request to create a new blob 2")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = $"/blobs/{blobId}",
                    Headers = new Dictionary<string, string>
                    {
                        {"Content-Type", "application/octet-stream"}
                    },
                    Body = bytes
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201
                });

            var consumer = new BlobsApiClient(ConsumerBlobApiPact.BaseUri);

            //Act / Assert
            consumer.CreateBlob(blobId, bytes, "test.txt");

            ConsumerBlobApiPact.Service.VerifyInteractions();
        }
    }
}