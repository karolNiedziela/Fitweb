using Backend.Core.Entities;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
            => await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri)
            {
                Content = Serialize(data)
            });

        public static async Task<T> CreatePostAsync<T>(this HttpClient httpClient, string requestUri, T data)
        {
            var response = await httpClient.PostAsync(requestUri, Serialize(data));

            var model = await response.ReadAsString<T>();

            return model;
        }

        public static async Task AuthenticateAsync(this HttpClient httpClient)
        {
            var signUpResponse = await httpClient.PostAsJsonAsync("https://localhost:5001/api/account/signup", new SignUp
            {
                Username = "testAdmin",
                Email = "testAdminEmail@email.com",
                Password = "testAdminSecret",
                Role = RoleId.Admin.ToString()
            });

            var resultSingUpRespnse = await signUpResponse.Content.ReadAsStringAsync();

            var loginResponse = await httpClient.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testAdmin",
                Password = "testAdminSecret"
            });

            var resultLoginResponse = await loginResponse.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<JwtDto>(resultLoginResponse);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);           
        }

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data),
            Encoding.UTF8, "application/json");
    }
}
