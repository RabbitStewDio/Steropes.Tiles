using System;

namespace Steropes.Tiles.MonogameDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new SimpleGame())
            {
                game.Run();
            }
        }
    }
}
