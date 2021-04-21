using Backend.Core.Entities;
using Backend.Core.Enums;
using Backend.Infrastructure.CommandQueryHandler.Commands;
using Backend.Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Helpers
{
    public static class HttpClientExtensions
    {
        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data),
            Encoding.UTF8, "application/json");

        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
            => await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri)
            {
                Content = Serialize(data)
            });

        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient httpClient, 
            string requestUri, T value)
        {
            var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("PATCH"),
                RequestUri = new Uri(httpClient.BaseAddress + requestUri),
                Content = content
            };

            return httpClient.SendAsync(request);
        }

        public static async Task<T> CreatePostAsync<T>(this HttpClient httpClient, string requestUri, T data)
        {
            var response = await httpClient.PostAsync(requestUri, Serialize(data));

            var model = await response.ReadAsString<T>();

            return model;
        }

        public static async Task AuthenticateAsync(this HttpClient httpClient)
        {
            var loginResponse = await httpClient.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testAdmin",
                Password = "testAdminSecret"
            });

            var model = await loginResponse.ReadAsString<JwtDto>();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);           
        }

        public static async Task AuthenticateUserAsync(this HttpClient httpClient)
        {
            var loginResponse = await httpClient.PostAsJsonAsync("https://localhost:5001/api/account/signin", new SignIn
            {
                Username = "testUser",
                Password = "testUserSecret"
            });

            var model = await loginResponse.ReadAsString<JwtDto>();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", model.AccessToken);
        } 


    }
}
