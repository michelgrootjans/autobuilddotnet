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
        private IMessageHandler handler2;

        [SetUp]
        public void Setup()
        {
            writer = MockRepository.GenerateMock<IWriter>();
            handler1 = MockRepository.GenerateMock<IMessageHandler>();
            handler2 = MockRepository.GenerateMock<IMessageHandler>();
            messageHandlers = new List<IMessageHandler>();
            parser = new MessageParser(null, messageHandlers);
        }

        [Test]
        public void OutputDataReceived_without_messagehandlers_does_nothing()
        {
            parser.OutputDataReceived("hello world");
        }

        [Test]
        public void OutputDataReceived_with_one_handler_calls_the_handler()
        {
            messageHandlers.Add(handler1);
            parser.OutputDataReceived("hello world");
            handler1.AssertWasCalled(h => h.Handle("hello world"));
        }

        [Test]
        public void OutputDataReceived_with_two_handlers_calls_both_handlers()
        {
            messageHandlers.Add(handler1);
            messageHandlers.Add(handler2);
            parser.OutputDataReceived("hello world");
            handler1.AssertWasCalled(h => h.Handle("hello world"));
            handler2.AssertWasCalled(h => h.Handle("hello world"));
        }

        [Test]
        public void OutputDataReceived_with_two_handler_when_one_handler_handles_the_message()
        {
            messageHandlers.Add(handler1);
            handler1.Stub(h => h.Handle("hello world")).Return(true);
            messageHandlers.Add(handler2);
            parser.OutputDataReceived("hello world");
            handler1.AssertWasCalled(h => h.Handle("hello world"));
            handler2.AssertWasNotCalled(h => h.Handle("hello world"));
        }
    }
}