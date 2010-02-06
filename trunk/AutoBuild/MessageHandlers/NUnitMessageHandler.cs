using System.Collections.Generic;

namespace AutoBuild.MessageHandlers
{
    internal class NUnitMessageHandler : IMessageHandler
    {
        private readonly IWriter writer;
        private readonly List<string> failures = new List<string>();

        public NUnitMessageHandler(IWriter writer)
        {
            this.writer = writer;
        }

        public bool Handle(string message)
        {
            if (CanHandle(message))
            {
                writer.WriteMessage(message);
                return true;
            }
            return false;
        }

        private void ParseFailures(string data)
        {
            const string literal = "Failures: ";
            var startindex = data.IndexOf(literal) + literal.Length;
            var endIndex = data.IndexOf(",", startindex);
            var numberOfFailures = data.Substring(startindex, endIndex - startindex);

            var parsedNumberOfFailures = int.Parse(numberOfFailures);

            for (var index = 1; index < parsedNumberOfFailures + 1; index++)
                failures.Add(string.Format("  {0})", index));
        }

        public void Dispose()
        {
        }

        private bool CanHandle(string message)
        {
            if (message.StartsWith("  Tests run"))
            {
                ParseFailures(message);
                return true;
            }

            if (failures.Exists(message.StartsWith))
            {
                return true;
            }
            if (message.StartsWith("  Test Case Failures"))
            {
                return true;
            }
            if (message.Contains("Expected:"))
            {
                return true;
            }
            if (message.Contains("But was:"))
            {
                return true;
            }
            if (message.Contains("---^"))
            {
                return true;
            }
            return false;
        }
    }
}