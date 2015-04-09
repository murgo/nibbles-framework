using System;
using System.Collections.Concurrent;

namespace NetworkEngine
{
    public interface IPipe<T>
    {
        event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;
        BlockingCollection<T> SendQueue { get; }
        BlockingCollection<T> ReceiveQueue { get; }
        void Receive(T o);
    }
}