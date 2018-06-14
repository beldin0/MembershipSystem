using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using MembershipSystem.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace MembershipSystem
{
    public class CustomAuthHandler : AuthenticationHandler<CustomAuthOptions>
    {
        public CustomAuthHandler(IOptionsMonitor<CustomAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Cannot read authentication header."));
            }

            string token = authorization.ToArray()[0];
            string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(token.Split(" ")[1]));
            string[] loginDetails = decoded.Split(':');
            string id = loginDetails[0];
            string pin = loginDetails[1];

            if (!Security.Login(id, pin))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid auth key."));
            }
            
            var identities = new List<ClaimsIdentity> {new ClaimsIdentity("basic auth")};
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identities), Options.Scheme);
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(id), null);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}