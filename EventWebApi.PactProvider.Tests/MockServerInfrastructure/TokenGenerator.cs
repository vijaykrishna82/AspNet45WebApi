using System;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;

namespace EventWebApi.PactProvider.Tests.MockServerInfrastructure
{
    public class TokenGenerator
    {
        private readonly IDataProtector DataProtector;

        public TokenGenerator(IDataProtector dataProtector)
        {
            DataProtector = dataProtector;
        }

        public string Generate()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "WebApiUser"),
                new Claim(ClaimTypes.Role, "User"),
                new Claim(ClaimTypes.Role, "PowerUser")
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var ticketDataFormat = new TicketDataFormat(DataProtector);
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties {ExpiresUtc = DateTime.UtcNow.AddHours(1)});
            var accessToken = ticketDataFormat.Protect(ticket);
            return accessToken;
        }
    }
}