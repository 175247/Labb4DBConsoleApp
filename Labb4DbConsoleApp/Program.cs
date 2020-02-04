using System;

namespace Labb4DbConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var modelContext = new GameContext();
            var controller = new Controller(modelContext);
            controller.Run();
        }
    }
}
