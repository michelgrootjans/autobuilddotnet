using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoBuild
{
    internal class Runner
    {
        private const string MS_BUILD_PATH = @"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe";
        List<string> failures;
        private bool startOutputting;
        private readonly Action<string> write;
        private readonly Action  clear;
        private string arguments;
        private MessageParser parser;

        public Runner(string arguments, Action<string> write, Action clear)
        {
            this.write = write;
            this.arguments = arguments;
            this.clear = clear;
            IsIdle = true;
            parser = new MessageParser(write, clear);
        }

        public bool IsIdle { get; private set; }

        public void Run(object state)
        {
            try
            {
                if (!IsIdle) return;
                IsIdle = false;
                
                write(string.Format("{0}: Starting build...", DateTime.Now.TimeOfDay));
                SartProcess();
            }
            catch (Exception e)
            {
                WriteError(e.ToString());
            }
            finally
            {
                IsIdle = true;
                write(string.Format("{0}: Build ended.", DateTime.Now.TimeOfDay));
            }
        }

        private void SartProcess()
        {
            var startInfo = new ProcessStartInfo(MS_BUILD_PATH)
                                {
                                    Arguments = arguments,
                                    UseShellExecute = false,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    RedirectStandardInput = true
                                };
            var msbuild = new Process {StartInfo = startInfo};
            msbuild.OutputDataReceived += parser.OutputDataReceived;
            msbuild.ErrorDataReceived += parser.ErrorDataReceived;
            msbuild.EnableRaisingEvents = true;

            msbuild.Start();
            msbuild.BeginOutputReadLine();
            msbuild.BeginErrorReadLine();
            msbuild.StandardInput.Close();

            msbuild.WaitForExit();
        }

        private void WriteError(string value)
        {
            write("Error received: " + value);
        }

    }
}