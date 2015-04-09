using System;
using System.Collections.Concurrent;

namespace NetworkEngine
{
    public class Pipe<T> : IPipe<T>
    {
        public event EventHandler<MessageReceivedEventArgs<T>> MessageReceived;

        public BlockingCollection<T> SendQueue { get; set; }
        public BlockingCollection<T> ReceiveQueue { get; set; }

        public Pipe()
        {
            SendQueue = new BlockingCollection<T>();
            ReceiveQueue = new BlockingCollection<T>();
        }

        public void Receive(T o)
        {
            ReceiveQueue.Add(o);
            OnMessageReceived(o);
        }

        protected virtual void OnMessageReceived(T e)
        {
            var handler = MessageReceived;
            if (handler != null) handler(this, new MessageReceivedEventArgs<T> { Message = e });
        }
    }
}