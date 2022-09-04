namespace Lab1_Tracer_.Core
{
    public interface ITracer
    {
        // Call at the beginning of the measured method
        void StartTrace();

        // Call at the end of the measured method
        void StopTrace();

        // Get measurement result
        TraceResult GetTraceResult();
    }
}
