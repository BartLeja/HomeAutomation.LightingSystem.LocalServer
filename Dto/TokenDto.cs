using Newtonsoft.Json;

namespace HomeAutomation.LightingSystem.LocalServer.Dto
{
    internal class TokenDto
    {
        [JsonProperty("Token")]
        public string Token { get; set; }
    }
}
