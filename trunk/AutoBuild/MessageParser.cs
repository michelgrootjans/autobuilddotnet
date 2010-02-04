using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoBuild
{
    internal class MessageParser
    {
        private readonly Action<string> write;
        private readonly Action clear;
        private bool startOutputting;
        private List<string> failures;

        public MessageParser(Action<string> write, Action clear)
        {
            this.write = write;
            this.clear = clear;
        }

        public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var value = e.Data;
            if (string.IsNullOrEmpty(value)) return;
            write(value);
        }

        public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (IsWorthPrinting(e.Data))
                write(e.Data.Trim());
        }

        private bool IsWorthPrinting(string data)
        {
            if (string.IsNullOrEmpty(data)) return false;


            if (data.StartsWith("  Tests run"))
            {
                startOutputting = true;
                failures = ParseFailures(data);
                return true;
            }
            if (!startOutputting) return false;

            if (failures.Exists(data.StartsWith)) return true;
            if (data.StartsWith("  Test Case Failures")) return true;
            if (data.Contains("Expected:")) return true;
            if (data.Contains("But was:")) return true;
            if (data.Contains("-----")) return true;
            return false;
        }

        private List<string> ParseFailures(string data)
        {
            const string literal = "Failures: ";
            var startindex = data.IndexOf(literal) + literal.Length;
            var endIndex = data.IndexOf(",", startindex);
            var numberOfFailures = data.Substring(startindex, endIndex - startindex);

            var list = new List<string>();

            var parsedNumberOfFailures = int.Parse(numberOfFailures);
            if (parsedNumberOfFailures == 0)
            {
                startOutputting = false;
                clear();
            }
            for (var index = 1; index < parsedNumberOfFailures + 1; index++)
                list.Add(string.Format("  {0})", index));

            return list;
        }
    }
}