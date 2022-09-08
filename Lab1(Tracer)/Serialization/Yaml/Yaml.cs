using Abstractions;
using Tracer.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Yaml
{
    public class Yaml : ITracerResultSerializer
    {
        public string Format
        {
            get
            {
                return "yaml";
            }
        }

        public void Serialize(TraceResult traceResult, Stream to)
        {
            List<ThreadInfo> threadsInfo = new List<ThreadInfo>();
            foreach (ThreadTrace thread in traceResult.Threads)
            {
                List<MethodInfo> rootMethods = new List<MethodInfo>();
                foreach (MethodTrace method in thread.Methods)
                {
                    rootMethods.Add(new MethodInfo(method.Name, method.Class, method.Time, MethodInfo.GetInnerMethods(method)));
                }
                threadsInfo.Add(new ThreadInfo(thread.ThreadID, thread.Time, rootMethods));
            }
            CustomTracerResult customTracerResult = new CustomTracerResult(threadsInfo);

            var serializer = new SerializerBuilder()
                            .WithNamingConvention(CamelCaseNamingConvention.Instance)
                            .Build();

            var yaml = serializer.Serialize(customTracerResult);

            using var sw = new StreamWriter(to);
            sw.Write(yaml);
            sw.Flush();
        }
    }
}