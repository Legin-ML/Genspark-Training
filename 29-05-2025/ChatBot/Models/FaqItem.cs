using System.Text.Json.Serialization;

namespace ChatBot.Models
{
    public class FaqItem
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }

}
