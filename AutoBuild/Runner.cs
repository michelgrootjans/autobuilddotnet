using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AutoBuild
{
    internal class Runner
    {
        private const string MS_BUILD_PATH = @"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe";
        private readonly Action<string> write;
        private readonly Action clear;
        private readonly string arguments;
        public bool IsIdle { get; private set; }

        public Runner(string arguments, Action<string> write, Action clear)
        {
            this.write = write;
            this.clear = clear;
            this.arguments = arguments;
            IsIdle = true;
        }

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
                write("Error received: " + e);
            }
            finally
            {
                IsIdle = true;
                write(string.Format("{0}: Build ended.", DateTime.Now.TimeOfDay));
            }
        }

        private void SartProcess()
        {
            using (var parser = new MessageParser(write, clear))
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
                parser.Dispose();
            }
        }
    }
}