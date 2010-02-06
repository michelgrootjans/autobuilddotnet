using System;
using System.Diagnostics;
using AutoBuild.MessageHandlers;

namespace AutoBuild
{
    internal class BuildRunner
    {
        private const string MS_BUILD_PATH = @"C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe";
        private readonly IWriter writer;
        private readonly string arguments;
        public bool IsRunning { get; private set; }

        public BuildRunner(string arguments, IWriter writer)
        {
            this.writer = writer;
            this.arguments = arguments;
            IsRunning = false;
        }

        public void Run(object state)
        {
            var startTime = DateTime.Now;
            try
            {
                if (IsRunning) return;
                IsRunning = true;
                writer.WriteMessage("Starting build...");
                SartProcess();
            }
            catch (Exception e)
            {
                writer.WriteError("Error received: " + e);
            }
            finally
            {
                var seconds = Math.Round((DateTime.Now - startTime).TotalSeconds);
                writer.WriteMessage(string.Format("Build ended (took {0} s).", seconds));
                IsRunning = false;
            }
        }

        private void SartProcess()
        {
            var handlers = new IMessageHandler[]
                               {
                                   new CompilerMessageHandler(writer),
                                   new MsBuildMessageHandler(writer),
                                   new NUnitMessageHandler(writer)
                               };

            using (var parser = new MessageParser(writer, handlers))
            {
                var startInfo = new ProcessStartInfo(MS_BUILD_PATH)
                                    {
                                        Arguments = arguments,
                                        UseShellExecute = false,
                                        RedirectStandardOutput = true,
                                        RedirectStandardError = true,
                                        RedirectStandardInput = true
                                    };
                var process = new Process {StartInfo = startInfo};
                process.OutputDataReceived += parser.OutputDataReceived;
                process.ErrorDataReceived += parser.ErrorDataReceived;
                process.EnableRaisingEvents = true;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.StandardInput.Close();

                process.WaitForExit();
            }
        }
    }
}