namespace Steropes.Tiles
{
    public interface ILogProvider
    {
        ILogAdapter CreateLogger(string traceName);
    }

    public static class LogProvider
    {
        static volatile ILogProvider logProvider;
        static DebugLogProvider fallbackProvider = new DebugLogProvider();

        public static ILogProvider Provider
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
            ILogProvider l = logProvider;
            if (l == null)
            {
                return fallbackProvider.CreateLogger(name);
            }

            return l.CreateLogger(name);
        }

        class DebugLogProvider : ILogProvider
        {
            public ILogAdapter CreateLogger(string traceName)
            {
                return new DebugLogAdapter(traceName);
            }
        }

        class DebugLogAdapter : ILogAdapter
        {
            readonly string traceName;

            public DebugLogAdapter(string traceName)
            {
                this.traceName = traceName;
            }

#if DEBUG
            public bool IsTraceEnabled
            {
                get
                {
                    return true;
                }
            }

            public void Trace(string message)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }

            public void Trace(string message, params object[] data)
            {
                System.Diagnostics.Debug.WriteLine(message, data);
            }
#else 
            public bool IsTraceEnabled
            {
                get
                {
                    return false;
                }
            }

            public void Trace(string message)
            {
            }

            public void Trace(string message, params object[] data)
            {
            }
#endif
        }
    }

    public interface ILogAdapter
    {
        bool IsTraceEnabled { get; }
        void Trace(string message);
        void Trace(string message, params object[] data);
    }
}