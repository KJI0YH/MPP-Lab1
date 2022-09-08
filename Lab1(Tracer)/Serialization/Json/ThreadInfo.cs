using System.Text.Json.Serialization;

namespace Json
{
    public class ThreadInfo
    {
        [JsonInclude, JsonPropertyName("id")]
        public int ID;

        [JsonInclude, JsonPropertyName("time")]
        public string Time;

        [JsonInclude, JsonPropertyName("methods")]
        public List<MethodInfo> Methods;

        public ThreadInfo(int id, double time, List<MethodInfo> methods)
        {
            ID = id;
            Time = String.Format("{0:f0}ms", time);
            Methods = methods;
        }
    }
}
