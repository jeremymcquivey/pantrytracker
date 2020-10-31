using System;

namespace PantryTrackers.Events
{
    public class BindableCollectionObjectSelectedArgs<T>: EventArgs
    {
        public BindableCollectionObjectSelectedArgs(T data)
        {
            Data = data;
        }

        public T Data { get; private set; }
    }
}
