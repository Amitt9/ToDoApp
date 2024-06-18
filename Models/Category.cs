using System.Text.Json.Serialization;

namespace ToDoApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        Work,
        Personal,
        Other 
    }
}
