using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoBuild.MessageHandlers;

namespace AutoBuild
{
    internal class MessageParser : IDisposable
    {
        private readonly IWriter writer;
        private readonly IEnumerable<IMessageHandler> messageHandlers;

        public MessageParser(IWriter writer, params IMessageHandler[] messageHandlers)
        {
            this.writer = writer;
            this.messageHandlers = new List<IMessageHandler>(messageHandlers);
        }

        public void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            writer.WriteError(e.Data);
        }

        public void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data)) return;
            Handle(e.Data);
        }

        private void Handle(string data)
        {
            foreach (var messageHandler in messageHandlers)
            {
                if (messageHandler.Handle(data)) break;
            }
#if DEBUG
            writer.WriteDebug(data);
#endif
        }


        public void Dispose()
        {
            foreach (var messageHandler in messageHandlers)
                messageHandler.Dispose();
        }
    }
}