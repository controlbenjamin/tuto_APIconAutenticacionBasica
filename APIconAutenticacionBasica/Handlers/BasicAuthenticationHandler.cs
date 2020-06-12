using APIconAutenticacionBasica.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace APIconAutenticacionBasica.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            try
            {

                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.Fail("Authorization header was not found!!!");
                }

                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);

                string credentials = Encoding.UTF8.GetString(bytes);

                string[] splitUserAndPass = credentials.Split(":");

                string username = splitUserAndPass[0];
                string password = splitUserAndPass[1];


                Usuarios user = Services.UsuariosDAL.GetUser(username, password);

                if (user == null)
                {
                    return AuthenticateResult.Fail("Invalid Username or Password!!!");
                }
                else
                {

                    var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);

                }

            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error has ocurred");
            }



        }
    }
}
