namespace AutoBuild.MessageHandlers
{
    internal class CompilerMessageHandler : IMessageHandler
    {
        private readonly IWriter writer;
        private bool compilerError;

        public CompilerMessageHandler(IWriter writer)
        {
            this.writer = writer;
        }

        public bool Handle(string message)
        {
            if (CanHandle(message))
            {
                writer.WriteError(message);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
        }

        private bool CanHandle(string message)
        {
            if (message.Contains("(CoreCompile target)"))
                compilerError = true;

            return compilerError;
        }
    }
}