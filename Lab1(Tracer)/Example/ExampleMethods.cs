using Tracer.Core;

namespace Example
{
    public class A
    {
        private ITracer _tracer;

        public A(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void A0()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            A1();
            _tracer.StopTrace();
        }

        public void A1()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            A2();
            _tracer.StopTrace();
        }

        public void A2()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }
    }

    public class B
    {
        private ITracer _tracer;

        public B(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void B0()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            B1();
            B2();
            _tracer.StopTrace();
        }

        public void B1()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }

        public void B2()
        {
            _tracer.StartTrace();
            Thread.Sleep(300);
            _tracer.StopTrace();
        }

    }
}
