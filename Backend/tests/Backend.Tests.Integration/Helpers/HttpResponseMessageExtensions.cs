using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ReadAsString<T>(this HttpResponseMessage httpResponseMessage)
        {
            var response = await httpResponseMessage.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<T>(response);

            return model;
        }
    }
}
