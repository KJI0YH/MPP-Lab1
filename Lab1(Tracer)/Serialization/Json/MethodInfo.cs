using System.Text.Json.Serialization;

namespace Json
{
    public class MethodInfo
    {
        [JsonInclude, JsonPropertyName("name")]
        public string Name;

        [JsonInclude, JsonPropertyName("class")]
        public string ClassName;

        [JsonInclude, JsonPropertyName("time")]
        public string Time;

        [JsonInclude, JsonPropertyName("methods")]
        public List<MethodInfo> Methods;

        public MethodInfo(string name, string className, TimeSpan time, List<MethodInfo> methods)
        {
            Name = name;
            ClassName = className;
            Time = String.Format("{0:f0}ms", time.TotalMilliseconds);
            Methods = methods;
        }

    }
}
