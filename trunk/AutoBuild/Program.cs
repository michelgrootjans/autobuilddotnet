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
            using (new Timer(RunIfNotBusy, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30)))
            {
                Console.ReadLine();
                if (buildRunner.IsRunning)
                    WillCloseWhenDone();
            }
        }

        private static void RunIfNotBusy(object state)
        {
            buildRunner.Run(state);
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