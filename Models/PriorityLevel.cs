using System.Text.Json.Serialization;

namespace ToDoApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }
}
