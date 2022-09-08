using Abstractions;
using Json;
using System.Text.Json;
using Tracer.Core;

public class JsonTracerResultSerializer : ITracerResultSerializer
{
    public string Format
    {
        get
        {
            return "json";
        }
    }

    public void Serialize(TraceResult traceResult, Stream to)
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

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

        JsonSerializer.Serialize(to, customTracerResult, options);
    }
}