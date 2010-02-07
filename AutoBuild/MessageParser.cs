using System;
using System.Collections.Generic;
using AutoBuild.MessageHandlers;

namespace AutoBuild
{
    public class MessageParser : IDisposable
    {
        private readonly IWriter writer;
        private readonly IEnumerable<IMessageHandler> messageHandlers;

        public MessageParser(IWriter writer, IEnumerable<IMessageHandler> messageHandlers)
        {
            this.writer = writer;
            this.messageHandlers = messageHandlers;
        }

        public void ErrorDataReceived(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            writer.WriteError(message);
        }

        public void OutputDataReceived(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Handle(message);
        }

        private void Handle(string message)
        {
            foreach (var messageHandler in messageHandlers)
            {
                if (messageHandler.Handle(message)) break;
            }
        }


        public void Dispose()
        {
            foreach (var messageHandler in messageHandlers)
                messageHandler.Dispose();
        }
    }
}