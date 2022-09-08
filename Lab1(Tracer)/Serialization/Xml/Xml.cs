using Abstractions;
using Tracer.Core;

public class Xml : ITracerResultSerializer
{
    public string Format
    {
        get
        {
            return "xml";
        }
    }

    public void Serialize(TraceResult traceResult, Stream to)
    {
        throw new NotImplementedException();
    }
}