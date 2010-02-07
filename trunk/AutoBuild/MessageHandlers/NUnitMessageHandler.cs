using System.Collections.Generic;

namespace AutoBuild.MessageHandlers
{
    internal class NUnitMessageHandler : IMessageHandler
    {
        private readonly IWriter writer;
        private readonly List<string> failedUnitTests = new List<string>();

        public NUnitMessageHandler(IWriter writer)
        {
            this.writer = writer;
        }

        public bool Handle(string message)
        {
            if (message.StartsWith("  Tests run"))
            {
                ParseFailures(message);
                if (failedUnitTests.Count == 0)
                    writer.WriteSuccess(message);
                else
                    writer.WriteError(message);
                return true;
            }
            if (CanHandle(message))
            {
                writer.WriteInfo(message);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
        }

        private bool CanHandle(string message)
        {

            if (failedUnitTests.Exists(message.StartsWith))
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

        private void ParseFailures(string data)
        {
            const string failures = "Failures: ";
            var startindex = data.IndexOf(failures) + failures.Length;
            var endIndex = data.IndexOf(",", startindex);
            var numberOfFailures = data.Substring(startindex, endIndex - startindex);

            var parsedNumberOfFailures = int.Parse(numberOfFailures);

            for (var index = 1; index < parsedNumberOfFailures + 1; index++)
                failedUnitTests.Add(string.Format("  {0})", index));
        }
    }
}