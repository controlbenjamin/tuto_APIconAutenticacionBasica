using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APIconAutenticacionBasica.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace APIconAutenticacionBasica.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CoronavirusController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CoronavirusController(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Countries>> GetCountriesAsync() {



            string url = _config["ApiCoVid19:Url"];

            string usuario = _config["ApiCoVid19:Usuario"];

            string password = _config["ApiCoVid19:Password"];

            string credenciales = usuario + ":" + password;

            var bytes = System.Text.Encoding.UTF8.GetBytes(credenciales);

            string authorization_header = "Basic " + Convert.ToBase64String(bytes);


            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };


            request.Headers.Add("Authorization", authorization_header);

            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            //este seria el string(json) que devuelve el api
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            //ahora debermos deserealizarlo
            List<Countries> listaPaises = JsonConvert.DeserializeObject<List<Countries>>(responseBody);

            return listaPaises;

        }

    }
}