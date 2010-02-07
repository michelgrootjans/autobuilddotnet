using System;
using System.Threading;

namespace AutoBuild
{
    internal class Program
    {
        private static BuildRunner buildRunner;

        private static void Main(string[] args)
        {
            buildRunner = new BuildRunner(args[0], new ConsoleWriter());
            using (new Timer(buildRunner.Run, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5)))
            {
                Console.ReadLine();
                if (buildRunner.IsRunning)
                    WillCloseWhenDone();
            }
        }

        private static void WillCloseWhenDone()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Build is running... will close when done.");
            Console.ForegroundColor = color;
        }
    }
}