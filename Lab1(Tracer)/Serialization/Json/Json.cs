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

    private List<MethodInfo> GetInnerMethods(MethodTrace method)
    {
        List<MethodInfo> innerMethods = new List<MethodInfo>();
        foreach (MethodTrace methodTrace in method.InnerMethods)
        {
            innerMethods.Add(new MethodInfo(methodTrace.Name, methodTrace.Class, methodTrace.Time, GetInnerMethods(methodTrace)));
        }
        return innerMethods;
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
            foreach (MethodTrace method in thread.Methods)
            {
                threadsInfo.Add(new ThreadInfo(thread.ThreadID, thread.Time, GetInnerMethods(method)));
            }
        }
        CustomTracerResult customTracerResult = new CustomTracerResult(threadsInfo);

        JsonSerializer.Serialize(to, customTracerResult, options);
    }
}