using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdmeliApp.Helpers
{
    public class Response
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "msj")]
        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
}
