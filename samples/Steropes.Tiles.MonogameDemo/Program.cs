using Microsoft.Extensions.Configuration;
using Serilog;
using System;

namespace Steropes.Tiles.MonogameDemo
{
    static class Program
    {
        static void SetUpLogging()
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", true)
                                .Build();

            var logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(configuration)
                         .CreateLogger();
            Log.Logger = logger;
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetUpLogging();
            SerilogLogAdapter.Install();
            
            using var game = new SimpleGame();
            game.Run();
        }
    }
}
