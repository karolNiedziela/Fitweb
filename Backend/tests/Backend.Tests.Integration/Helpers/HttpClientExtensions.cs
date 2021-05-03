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

            var model = await response.Content.ReadAsAsync<T>();

            return model;
        }   

    }
}
