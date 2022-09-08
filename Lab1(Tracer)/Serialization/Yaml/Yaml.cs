using Abstractions;
using Tracer.Core;

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
        throw new NotImplementedException();
    }
}