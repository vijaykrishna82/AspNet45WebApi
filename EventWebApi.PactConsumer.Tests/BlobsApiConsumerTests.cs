using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventWebApi.Client;
using EventWebApi.PactConsumer.Tests.MockPactProviders;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace EventWebApi.PactConsumer.Tests
{
    public class BlobsApiConsumerTests :  IUseFixture<ConsumerBlobApiPact>
    {
        private IMockProviderService MockProviderService;
        private string MockProviderServiceBaseUri;
            
        public void SetFixture(ConsumerBlobApiPact data)
        {
            MockProviderService = data.Service;
            MockProviderServiceBaseUri = data.BaseUri;
            MockProviderService.ClearInteractions();
        }

        [Fact]
        public void CreateBlob_WhenCalledWithBlob_Succeeds()
        {
            //Arrange
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            var bytes = Encoding.UTF8.GetBytes("This is a test");

            MockProviderService.UponReceiving("a request to create a new blob")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = $"/blobs/{blobId}",
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "application/octet-stream" }
                    },
                    Body = bytes
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201
                });

            var consumer = new BlobsApiClient(MockProviderServiceBaseUri);

            //Act / Assert
            consumer.CreateBlob(blobId, bytes, "test.txt");

            MockProviderService.VerifyInteractions();
        }

        [Fact]
        public void GetBlob_WhenCalledWithId_Succeeds()
        {
            //Arrange
            var blobId = Guid.Parse("38C3976B-5AE8-4F2F-A8EC-46F6AEE826E2");
            var bytes = Encoding.UTF8.GetBytes("This is a test");

            MockProviderService.UponReceiving("a request to get a new blob by id")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = string.Format("/blobs/{0}", blobId)
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201,
                    Headers = new Dictionary<string, string>
                    {
                        { "Content-Type", "text/plain" }
                    },
                    Body = "This is a test"
                });

            var consumer = new BlobsApiClient(MockProviderServiceBaseUri);

            //Act / Assert
            var content = consumer.GetBlob(blobId);

            Assert.True(bytes.SequenceEqual(content));

            MockProviderService.VerifyInteractions();
        }
    }
}
