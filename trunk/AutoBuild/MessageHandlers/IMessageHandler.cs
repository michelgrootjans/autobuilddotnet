using System;

namespace AutoBuild.MessageHandlers
{
    public interface IMessageHandler : IDisposable
    {
        bool Handle(string message);
    }
}