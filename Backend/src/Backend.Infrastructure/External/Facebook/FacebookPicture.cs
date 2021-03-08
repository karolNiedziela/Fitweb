using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.External
{
    public class FacebookPicture
    {
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
