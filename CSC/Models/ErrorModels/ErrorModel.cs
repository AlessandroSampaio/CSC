using Newtonsoft.Json;

namespace CSC.Models.ErrorModels
{
    public class ErrorModel
    {
        [JsonProperty("status")]
        public string status { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }
    }
}
