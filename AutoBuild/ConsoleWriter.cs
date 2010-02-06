using System;

namespace AutoBuild
{
    internal class ConsoleWriter : IWriter
    {
        private readonly ConsoleColor orginalConsoleColor;

        public ConsoleWriter()
        {
            orginalConsoleColor = Console.ForegroundColor;
        }

        public void WriteTitle(string message)
        {
            WriteMessage(ConsoleColor.Yellow, message);
        }

        public void WriteError(string message)
        {
            WriteMessage(ConsoleColor.Red, message);
        }

        public void WriteMessage(string message)
        {
            WriteMessage(ConsoleColor.White, message);
        }

        public void WriteDebug(string message)
        {
            WriteMessage(ConsoleColor.Gray, message);
        }

        private void WriteMessage(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = orginalConsoleColor;
        }
    }
}