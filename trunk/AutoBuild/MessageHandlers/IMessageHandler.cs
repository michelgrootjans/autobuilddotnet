using System;

namespace AutoBuild.MessageHandlers
{
    internal interface IMessageHandler : IDisposable
    {
        bool Handle(string message);
    }
}