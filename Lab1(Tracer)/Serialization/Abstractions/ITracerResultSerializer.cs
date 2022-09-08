using Tracer.Core;

namespace Abstractions
{
    public interface ITracerResultSerializer
    {
        string Format { get; }
        void Serialize(TraceResult traceResult, Stream to);
    }
}
