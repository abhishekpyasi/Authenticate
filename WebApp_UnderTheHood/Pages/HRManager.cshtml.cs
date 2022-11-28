using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using WebApp_UnderTheHood.Authorization;
using WebApp_UnderTheHood.Dto;
using WebApp_UnderTheHood.Pages.Account;

namespace WebApp_UnderTheHood.Pages
{
    [Authorize(Policy = "HRManagerOnly")]
    public class HRManagerModel : PageModel
    {

        [BindProperty]

        public List<WeatherForecastDto> weatherForecastItems { get; set; }
        public HRManagerModel(IHttpClientFactory httpClientFactory )
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IHttpClientFactory httpClientFactory { get; }

        public async Task OnGetAsync()


        {
             
            //get token from session

            JwtToken token = null;

            var strTokenObj = HttpContext.Session.GetString("access_token");


            if (string.IsNullOrWhiteSpace(strTokenObj))             // if token is null call webAPI end point

            {


                token =await  Authenticate(); // 
            }
            else
            {

                token = JsonConvert.DeserializeObject<Authorization.JwtToken>(strTokenObj);
            }

            if (token ==  null || string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpireAt < DateTime.UtcNow )
            {
                token = await Authenticate();

            }


            var httpClient = httpClientFactory.CreateClient("OurWebApi");
            //Adding token to headers of httpclient GET call


            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.
                AuthenticationHeaderValue("Bearer", token.AccessToken);
            // calling webapi from application
            weatherForecastItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDto>>("/WeatherForecast");

        }

        private async Task<JwtToken> Authenticate()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebApi");

            //post to end point with credential to authenticate and create token and storing response in res variable
            var res = await httpClient.PostAsJsonAsync("auth", new Credential { UserName = "admin", Password = "password" });

            //http response message if aabove call is successful
            res.EnsureSuccessStatusCode();
            // to get http response message as string 
            string strjwt = await res.Content.ReadAsStringAsync();

            HttpContext.Session.SetString("access_token", strjwt);
            // convert string to a .NET type
           return   JsonConvert.DeserializeObject<JwtToken>(strjwt);


        }
    }
}
