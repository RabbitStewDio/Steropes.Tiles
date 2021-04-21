using Avalonia.Controls;
using Avalonia.Logging;
using Serilog;
using Serilog.Filters;
using SerilogLogEventLevel = Serilog.Events.LogEventLevel;

namespace Steropes.Tiles.TemplateGen
{
    public static class SerilogExtensions
    {
        const string DefaultTemplate = "[{Area}] {Message} ({SourceType} #{SourceHash})";

        public static T LogToSerilog<T>(this T builder, ILogger baseLogger)
            where T : AppBuilderBase<T>, new()
        {
            SerilogLogger.Initialize(baseLogger);
            return builder;
        }

        /// <summary>
        /// Logs Avalonia events to the <see cref="System.Diagnostics.Debug"/> sink.
        /// </summary>
        /// <typeparam name="T">The application class type.</typeparam>
        /// <param name="builder">The app builder instance.</param>
        /// <param name="level">The minimum level to log.</param>
        /// <returns>The app builder instance.</returns>
        public static T LogToDebug<T>(
            this T builder,
            LogEventLevel level = LogEventLevel.Warning)
            where T : AppBuilderBase<T>, new()
        {
            SerilogLogger.Initialize(new LoggerConfiguration()
                                     .MinimumLevel.Is((SerilogLogEventLevel)level)
                                     .Enrich.FromLogContext()
                                     .WriteTo.Debug(outputTemplate: DefaultTemplate)
                                     .CreateLogger());
            return builder;
        }

        /// <summary>
        /// Logs Avalonia events to the <see cref="System.Diagnostics.Debug"/> sink.
        /// </summary>
        /// <typeparam name="T">The application class type.</typeparam>
        /// <param name="builder">The app builder instance.</param>
        /// <param name="area">The area to log. Valid values are listed in <see cref="LogArea"/>.</param>
        /// <param name="level">The minimum level to log.</param>
        /// <returns>The app builder instance.</returns>
        public static T LogToDebug<T>(
            this T builder,
            string area,
            LogEventLevel level = LogEventLevel.Warning)
            where T : AppBuilderBase<T>, new()
        {
            SerilogLogger.Initialize(new LoggerConfiguration()
                                     .MinimumLevel.Is((SerilogLogEventLevel)level)
                                     .Filter.ByIncludingOnly(Matching.WithProperty("Area", area))
                                     .Enrich.FromLogContext()
                                     .WriteTo.Debug(outputTemplate: DefaultTemplate)
                                     .CreateLogger());
            return builder;
        }
    }
}
