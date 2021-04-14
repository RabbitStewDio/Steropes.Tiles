using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Steropes.Tiles.MonogameDemo
{
    class SerilogLogAdapter : LogAdapterBase
    {
        readonly ILogger logger;

        public SerilogLogAdapter(ILogger logger)
        {
            this.logger = logger;
        }

        public override bool IsTraceEnabled => logger.IsEnabled(LogEventLevel.Debug);

        public override void Trace(string message)
        {
            logger.Debug("{Message}", message);
        }

        public static void Install()
        {
            LogProvider.Provider = x => new SerilogLogAdapter(Log.ForContext(Constants.SourceContextPropertyName, x));
        }
    }
}
