namespace Yaml
{
    public class CustomTracerResult
    {
        public List<ThreadInfo> Threads = new();

        public CustomTracerResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }
    }
}
