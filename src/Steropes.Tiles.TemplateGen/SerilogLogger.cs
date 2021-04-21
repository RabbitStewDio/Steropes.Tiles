using Avalonia.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Steropes.Tiles.TemplateGen
{
    [SuppressMessage("ReSharper", "TemplateIsNotCompileTimeConstantProblem")]
    public class SerilogLogger : ILogSink
    {
        readonly ILogger output;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerilogLogger"/> class.
        /// </summary>
        /// <param name="output">The serilog logger to use.</param>
        public SerilogLogger(ILogger output)
        {
            this.output = output;
        }

        /// <summary>
        /// Initializes the Avalonia logging with a new instance of a <see cref="SerilogLogger"/>.
        /// </summary>
        /// <param name="output">The serilog logger to use.</param>
        public static void Initialize(ILogger output)
        {
            Logger.Sink = new SerilogLogger(output);
        }

        public bool IsEnabled(LogEventLevel level, string area)
        {
            return output.IsEnabled((Serilog.Events.LogEventLevel)level);
        }

        public void Log(LogEventLevel level,
                        string area,
                        object source,
                        string messageTemplate)
        {
            using (PushLogContextProperties(area, source))
            {
                output.Write((Serilog.Events.LogEventLevel)level, messageTemplate);
            }
        }

        public void Log<T0>(LogEventLevel level,
                            string area,
                            object source,
                            string messageTemplate,
                            T0 propertyValue0)
        {
            using (PushLogContextProperties(area, source))
            {
                output.Write((Serilog.Events.LogEventLevel)level, messageTemplate, propertyValue0);
            }
        }

        public void Log<T0, T1>(LogEventLevel level,
                                string area,
                                object source,
                                string messageTemplate,
                                T0 propertyValue0,
                                T1 propertyValue1)
        {
            using (PushLogContextProperties(area, source))
            {
                output.Write((Serilog.Events.LogEventLevel)level, messageTemplate, propertyValue0, propertyValue1);
            }
        }

        public void Log<T0, T1, T2>(LogEventLevel level,
                                    string area,
                                    object source,
                                    string messageTemplate,
                                    T0 propertyValue0,
                                    T1 propertyValue1,
                                    T2 propertyValue2)
        {
            using (PushLogContextProperties(area, source))
            {
                output.Write((Serilog.Events.LogEventLevel)level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
            }
        }

        /// <inheritdoc/>
        public void Log(LogEventLevel level,
                        string area,
                        object source,
                        string messageTemplate,
                        params object[] propertyValues)
        {
            using (PushLogContextProperties(area, source))
            {
                output.Write((Serilog.Events.LogEventLevel)level, messageTemplate, propertyValues);
            }
        }

        static LogContextDisposable PushLogContextProperties(string area, object? source)
        {
            if (source == null)
            {
                return new LogContextDisposable(
                    LogContext.PushProperty("Area", area),
                    LogContext.PushProperty("SourceType", null),
                    LogContext.PushProperty("SourceHash", null)
                );
            }
            
            var hc = source.GetHashCode();
            return new LogContextDisposable(
                LogContext.PushProperty("Area", area),
                LogContext.PushProperty("SourceType", source.GetType()),
                LogContext.PushProperty("SourceHash", hc)
            );
        }

        readonly struct LogContextDisposable : IDisposable
        {
            readonly IDisposable areaDisposable;
            readonly IDisposable sourceTypeDisposable;
            readonly IDisposable sourceHashDisposable;

            public LogContextDisposable(IDisposable areaDisposable, IDisposable sourceTypeDisposable, IDisposable sourceHashDisposable)
            {
                this.areaDisposable = areaDisposable;
                this.sourceTypeDisposable = sourceTypeDisposable;
                this.sourceHashDisposable = sourceHashDisposable;
            }

            public void Dispose()
            {
                areaDisposable.Dispose();
                sourceTypeDisposable.Dispose();
                sourceHashDisposable.Dispose();
            }
        }
    }
}
