using System;
using System.Threading;

namespace AutoBuild
{
    internal class Program
    {
        private static Runner runner;

        private static void Main(string[] args)
        {
            runner = new Runner(args[0], Console.WriteLine, Console.Clear);
            using (new Timer(RunIfNotBusy, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(20)))
            {
                Console.ReadLine();
            }
        }

        private static void RunIfNotBusy(object state)
        {
            if(runner.IsIdle)
                runner.Run(state);
        }
    }
}