using System.Text.Json.Serialization;

namespace Json
{
    public class CustomTracerResult
    {
        [JsonInclude, JsonPropertyName("threads")]
        public List<ThreadInfo> Threads = new();

        public CustomTracerResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }
    }
}
