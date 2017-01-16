using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin.Security.DataProtection;

namespace EventWebApi.PactProvider.Tests.MockServerInfrastructure
{
    public class AuthorizationTokenReplacementMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> Next;
        private readonly TokenGenerator TokenGenerator;

        public AuthorizationTokenReplacementMiddleware(Func<IDictionary<string, object>, Task> next, IDataProtector dataProtector)
        {
            Next = next;
            TokenGenerator = new TokenGenerator(dataProtector);
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var headers = environment["owin.RequestHeaders"] as IDictionary<string, string[]>;

            Debug.Assert(headers != null, "headers != null");

            if (headers.ContainsKey("Authorization") && headers["Authorization"][0] == "Bearer someValidAuthToken")
            {
                headers["Authorization"][0] = $"Bearer {TokenGenerator.Generate()}";
            }

            await Next.Invoke(environment);
        }
    }
}
