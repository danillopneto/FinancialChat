using Newtonsoft.Json;

namespace Jobsity.Challenge.FinancialChat.Bot.Models
{
    public class CommandDto
    {
        public Guid Destination { get; set; }

        [JsonProperty("message")]
        public string Command { get; set; }

        public string CommandParameter;

        public Guid SenderId { get; set; }
    }
}