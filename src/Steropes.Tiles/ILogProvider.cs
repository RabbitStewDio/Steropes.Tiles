namespace Steropes.Tiles
{
    public delegate ILogAdapter LogProviderFunction(string traceName); 

    public abstract class LogAdapterBase : ILogAdapter
    {
        public abstract bool IsTraceEnabled { get; }

        public abstract void Trace(string message);

        public virtual void Trace<T>(string message, T data)
        {
            Trace(string.Format(message, data));
        }

        public virtual void Trace<T1, T2>(string message, T1 data1, T2 data2)
        {
            Trace(string.Format(message, data1, data2));
        }

        public virtual void Trace<T1, T2, T3>(string message, T1 data1, T2 data2, T3 data3)
        {
            Trace(string.Format(message, data1, data2, data3));
        }

        public virtual void Trace(string message, params object[] data)
        {
            Trace(string.Format(message, data));
        }
    }
    
    public static class LogProvider
    {
        static volatile LogProviderFunction logProvider;
        public static LogProviderFunction Provider
        {
            get { return logProvider; }
            set { logProvider = value; }
        }

        public static ILogAdapter CreateLogger<TLoggerType>()
        {
            return CreateLogger(TracingUtil.NameWithoutGenerics(typeof(TLoggerType)));
        }

        public static ILogAdapter CreateLogger(string name)
        {
            LogProviderFunction l = logProvider;
            if (l == null)
            {
                return CreateFallbackLogger(name);
            }

            return l(name);
        }

        static ILogAdapter CreateFallbackLogger(string traceName)
        {
            return new DebugLogAdapter(traceName);
        }

        class DebugLogAdapter : LogAdapterBase
        {
            readonly string traceName;

            public DebugLogAdapter(string traceName)
            {
                this.traceName = traceName;
            }
#if DEBUG
            public override bool IsTraceEnabled
            {
                get
                {
                    return true;
                }
            }

            public override void Trace(string message)
            {
                System.Diagnostics.Debug.WriteLine($"[{traceName}] {message}");
            }
#else
            public override bool IsTraceEnabled
            {
                get
                {
                    return false;
                }
            }

            public override void Trace(string message)
            {
            }
#endif
        }
    }

    public interface ILogAdapter
    {
        bool IsTraceEnabled { get; }
        void Trace(string message);
        void Trace<T>(string message, T data);
        void Trace<T1, T2>(string message, T1 data, T2 data2);
        void Trace<T1, T2, T3>(string message, T1 data, T2 data2, T3 data3);
        void Trace(string message, params object[] data);
    }
}
