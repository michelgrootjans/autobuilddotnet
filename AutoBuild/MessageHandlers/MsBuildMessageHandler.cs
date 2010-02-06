namespace AutoBuild.MessageHandlers
{
    internal class MsBuildMessageHandler : IMessageHandler
    {
        private readonly IWriter writer;

        public MsBuildMessageHandler(IWriter writer)
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

        public void Dispose()
        {
        }

        private static bool CanHandle(string message)
        {
            return message.Contains("Warning(s)") || message.Contains("Error(s)");
        }
    }
}