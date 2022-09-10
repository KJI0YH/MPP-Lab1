using Abstractions;
using System.Reflection;
using Tracer.Core;

namespace Example
{
    public class Example
    {
        private string _serializersPath = "Plugins/Serializers";
        private List<ITracerResultSerializer> _serializers = new();

        public static void Main()
        {
            Example example = new Example();
            Tracer.Core.Tracer tracer = new Tracer.Core.Tracer();

            A a = new A(tracer);
            B b = new B(tracer);

            var t1 = new Thread(() =>
            {
                a.A0();
                a.A1();
                a.A2();
            });
            t1.Start();

            var t2 = new Thread(() =>
            {
                b.B0();

            });
            t2.Start();

            t1.Join();
            t2.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            List<ITracerResultSerializer> serializers = example.RefreshSerializers();
            foreach (var serializer in serializers)
            {
                using var file = new FileStream($"result.{serializer.Format}", FileMode.Create);
                serializer.Serialize(traceResult, file);
            }
        }

        public List<ITracerResultSerializer> RefreshSerializers()
        {
            _serializers.Clear();

            DirectoryInfo pluginDirectory = new DirectoryInfo(_serializersPath);
            if (!pluginDirectory.Exists)
                pluginDirectory.Create();


            var pluginFiles = Directory.GetFiles(_serializersPath, "*.dll");
            foreach (var file in pluginFiles)
            {

                Assembly asm = Assembly.LoadFrom(file);

                var types = asm.GetTypes().
                                Where(t => t.GetInterfaces().
                                Where(i => i.FullName == typeof(ITracerResultSerializer).FullName).Any());


                foreach (var type in types)
                {
                    ITracerResultSerializer? serializer = asm.CreateInstance(type.FullName) as ITracerResultSerializer;

                    if (serializer != null)
                    {
                        _serializers.Add(serializer);
                    }

                }
            }
            return _serializers;
        }
    }
}