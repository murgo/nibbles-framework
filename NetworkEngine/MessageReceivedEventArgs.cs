using System;

namespace NetworkEngine
{
    public class MessageReceivedEventArgs<T> : EventArgs
    {
        public T Message { get; set; }
    }
}