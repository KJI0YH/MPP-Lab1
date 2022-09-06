using Tracer.Core;

namespace Tracer.Serialization.Abstractions
{
    public interface ITracerResultSerializer
    {
        string Format { get; }
        void Serialize(TraceResult traceResult, Stream to);
    }
}
