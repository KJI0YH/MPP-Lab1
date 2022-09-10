using Example;
using Tracer.Core;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SingleThreadWithNoInnerMethods()
        {
            // Arrange
            ITracer tracer = new Tracer.Core.Tracer();
            A a = new A(tracer);

            // Act
            a.A2();
            TraceResult traceResult = tracer.GetTraceResult();

            // Assert
            Assert.That(traceResult.Threads.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].Name, Is.EqualTo("A2"));
                Assert.That(traceResult.Threads[0].Methods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].Time.TotalMilliseconds, Is.InRange(300, 400));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods.Count, Is.EqualTo(0));
            });
            Assert.That(traceResult.Threads[0].Time, Is.InRange(300, 400));
        }

        [Test]
        public void SingleThreadWithInnerMethods()
        {
            // Arrange
            ITracer tracer = new Tracer.Core.Tracer();
            A a = new A(tracer);

            // Act
            a.A1();
            TraceResult traceResult = tracer.GetTraceResult();

            // Assert
            Assert.That(traceResult.Threads.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].Name, Is.EqualTo("A1"));
                Assert.That(traceResult.Threads[0].Methods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].Time.TotalMilliseconds, Is.InRange(500, 600));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods.Count, Is.EqualTo(1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Name, Is.EqualTo("A2"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Time.TotalMilliseconds, Is.InRange(300, 400));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods.Count, Is.EqualTo(0));
            });
            Assert.That(traceResult.Threads[0].Time, Is.InRange(500, 600));
        }

        [Test]
        public void SingleThreadSameLevelMethods()
        {
            // Arrange
            ITracer tracer = new Tracer.Core.Tracer();
            A a = new A(tracer);
            B b = new B(tracer);

            // Act
            a.A2();
            b.B2();
            TraceResult traceResult = tracer.GetTraceResult();

            // Assert
            Assert.That(traceResult.Threads.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].Name, Is.EqualTo("A2"));
                Assert.That(traceResult.Threads[0].Methods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].Time.TotalMilliseconds, Is.InRange(300, 400));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods.Count, Is.EqualTo(0));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[1].Name, Is.EqualTo("B2"));
                Assert.That(traceResult.Threads[0].Methods[1].Class, Is.EqualTo("B"));
                Assert.That(traceResult.Threads[0].Methods[1].Time.TotalMilliseconds, Is.InRange(200, 300));
                Assert.That(traceResult.Threads[0].Methods[1].InnerMethods.Count, Is.EqualTo(0));

            });
            Assert.That(traceResult.Threads[0].Time, Is.InRange(500, 600));
        }

        [Test]
        public void MultiThreadWithNoInnerMethods()
        {
            // Arrange
            ITracer tracer = new Tracer.Core.Tracer();
            A a = new A(tracer);
            B b = new B(tracer);

            // Act
            var t1 = new Thread(() =>
            {
                a.A2();
            });

            var t2 = new Thread(() =>
            {
                b.B2();
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            // Assert
            Assert.That(traceResult.Threads.Count, Is.EqualTo(2));
            Assert.That(traceResult.Threads[0].Methods.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].Name, Is.EqualTo("A2"));
                Assert.That(traceResult.Threads[0].Methods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].Time.TotalMilliseconds, Is.InRange(300, 400));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods.Count, Is.EqualTo(0));
            });
            Assert.That(traceResult.Threads[0].Time, Is.InRange(300, 400));

            Assert.That(traceResult.Threads[1].Methods.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[1].Methods[0].Name, Is.EqualTo("B2"));
                Assert.That(traceResult.Threads[1].Methods[0].Class, Is.EqualTo("B"));
                Assert.That(traceResult.Threads[1].Methods[0].Time.TotalMilliseconds, Is.InRange(200, 300));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods.Count, Is.EqualTo(0));
            });

            Assert.That(traceResult.Threads[1].Time, Is.InRange(200, 300));
        }

        [Test]
        public void MultiThreadWithInnerMethods()
        {
            // Arrange
            ITracer tracer = new Tracer.Core.Tracer();
            A a = new A(tracer);
            B b = new B(tracer);

            // Act
            var t1 = new Thread(() =>
            {
                a.A0();
            });

            var t2 = new Thread(() =>
            {
                b.B0();
            });

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            TraceResult traceResult = tracer.GetTraceResult();

            // Assert
            Assert.That(traceResult.Threads.Count, Is.EqualTo(2));
            Assert.That(traceResult.Threads[0].Methods.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].Name, Is.EqualTo("A0"));
                Assert.That(traceResult.Threads[0].Methods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].Time.TotalMilliseconds, Is.InRange(600, 700));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods.Count, Is.EqualTo(1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Name, Is.EqualTo("A1"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].Time.TotalMilliseconds, Is.InRange(500, 600));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods.Count, Is.EqualTo(1));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods[0].Name, Is.EqualTo("A2"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods[0].Class, Is.EqualTo("A"));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods[0].Time.TotalMilliseconds, Is.InRange(300, 400));
                Assert.That(traceResult.Threads[0].Methods[0].InnerMethods[0].InnerMethods[0].InnerMethods.Count, Is.EqualTo(0));
            });

            Assert.That(traceResult.Threads[0].Time, Is.InRange(600, 700));

            Assert.That(traceResult.Threads[1].Methods.Count, Is.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[1].Methods[0].Name, Is.EqualTo("B0"));
                Assert.That(traceResult.Threads[1].Methods[0].Class, Is.EqualTo("B"));
                Assert.That(traceResult.Threads[1].Methods[0].Time.TotalMilliseconds, Is.InRange(500, 600));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods.Count, Is.EqualTo(2));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[0].Name, Is.EqualTo("B1"));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[0].Class, Is.EqualTo("B"));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[0].Time.TotalMilliseconds, Is.InRange(200, 400));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[0].InnerMethods.Count, Is.EqualTo(0));
            });

            Assert.Multiple(() =>
            {
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[1].Name, Is.EqualTo("B2"));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[1].Class, Is.EqualTo("B"));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[1].Time.TotalMilliseconds, Is.InRange(200, 400));
                Assert.That(traceResult.Threads[1].Methods[0].InnerMethods[1].InnerMethods.Count, Is.EqualTo(0));
            });

            Assert.That(traceResult.Threads[1].Time, Is.InRange(500, 600));
        }
    }
}