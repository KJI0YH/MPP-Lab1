using System.Text.Json.Serialization;
using Tracer.Core;

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

        public static List<MethodInfo> GetInnerMethods(MethodTrace method)
        {
            List<MethodInfo> innerMethods = new List<MethodInfo>();
            foreach (MethodTrace methodTrace in method.InnerMethods)
            {
                innerMethods.Add(new MethodInfo(methodTrace.Name, methodTrace.Class, methodTrace.Time, GetInnerMethods(methodTrace)));
            }
            return innerMethods;
        }
    }
}
