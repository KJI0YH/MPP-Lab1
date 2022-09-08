namespace Yaml
{
    public class CustomTracerResult
    {
        public List<ThreadInfo> Threads;

        public CustomTracerResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }
    }
}
