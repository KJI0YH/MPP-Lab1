using Abstractions;
using System.Xml;
using System.Xml.Serialization;
using Tracer.Core;

namespace Xml
{
    public class XmlTracerResultSerializer : ITracerResultSerializer
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
            List<ThreadInfo> threadsInfo = new List<ThreadInfo>();
            List<MethodInfo> rootMethods = new List<MethodInfo>();
            foreach (ThreadTrace thread in traceResult.Threads)
            {
                rootMethods.Clear();
                foreach (MethodTrace method in thread.Methods)
                {
                    rootMethods.Add(new MethodInfo(method.Name, method.Class, method.Time, MethodInfo.GetInnerMethods(method)));
                }
                threadsInfo.Add(new ThreadInfo(thread.ThreadID, thread.Time, rootMethods));
            }
            CustomTracerResult customTracerResult = new CustomTracerResult(threadsInfo);

            using var xmlWriter = XmlWriter.Create(to, new XmlWriterSettings { Indent = true });
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomTracerResult));
            xmlSerializer.Serialize(xmlWriter, customTracerResult);
        }
    }
}