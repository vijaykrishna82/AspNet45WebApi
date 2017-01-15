using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EventWebApi.Client
{
    public class BlobsApiClient : ApiClient
    {
        public BlobsApiClient(string baseUri = null, string authToken = null) : base(baseUri, authToken) { }

        public void CreateBlob(Guid id, byte[] content, string fileName)
        {
            var bytes = new ByteArrayContent(content);
            bytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("file") { FileName = fileName };
            bytes.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");


            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUri}/blobs/{id}") { Content = bytes };

            var response = HttpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                var statusCode = result.StatusCode;
                if (statusCode == HttpStatusCode.Created)
                {
                    return;
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }
        }

        public byte[] GetBlob(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUri}/blobs/{id}");

            var response = HttpClient.SendAsync(request);

            try
            {
                var result = response.Result;
                var statusCode = result.StatusCode;
                if (statusCode == HttpStatusCode.Created)
                {
                    return result.Content.ReadAsByteArrayAsync().Result;
                }

                RaiseResponseError(request, result);
            }
            finally
            {
                Dispose(request, response);
            }

            return null;
        }
    }
}
