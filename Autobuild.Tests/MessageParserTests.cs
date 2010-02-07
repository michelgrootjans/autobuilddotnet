using System.Collections.Generic;
using System.Diagnostics;
using AutoBuild;
using AutoBuild.MessageHandlers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Autobuild.Tests
{
    [TestFixture]
    public class MessageParserTests
    {
        private MessageParser parser;
        private IWriter writer;
        private List<IMessageHandler> messageHandlers;
        private IMessageHandler handler1;

        [SetUp]
        public void Setup()
        {
            writer = MockRepository.GenerateMock<IWriter>();
            handler1 = MockRepository.GenerateMock<IMessageHandler>();
            messageHandlers = new List<IMessageHandler>();
            parser = new MessageParser(writer, messageHandlers);
        }

        [Test]
        public void blah()
        {
            parser.OutputDataReceived("hello world");
            writer.AssertWasNotCalled(w => w.WriteDebug(Arg<string>.Is.Anything));
            writer.AssertWasNotCalled(w => w.WriteError(Arg<string>.Is.Anything));
            writer.AssertWasNotCalled(w => w.WriteInfo(Arg<string>.Is.Anything));
            writer.AssertWasNotCalled(w => w.WriteSuccess((Arg<string>.Is.Anything)));
            writer.AssertWasNotCalled(w => w.WriteTitle(Arg<string>.Is.Anything));
        }

        [Test]
        public void OutputDataReceived_with_one_handler_calls_the_handler()
        {
            messageHandlers.Add(handler1);
            parser.OutputDataReceived("hello world");
            handler1.AssertWasCalled(h => h.Handle("hello world"));
        }


    }
}