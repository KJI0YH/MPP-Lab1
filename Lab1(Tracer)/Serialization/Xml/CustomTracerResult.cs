using System.Xml.Serialization;

namespace Xml
{
    [XmlRoot("root")]
    public class CustomTracerResult
    {
        [XmlElement("threads")]
        public List<ThreadInfo> Threads = new();

        public CustomTracerResult(List<ThreadInfo> threads)
        {
            Threads = threads;
        }

        public CustomTracerResult()
        {

        }
    }
}
