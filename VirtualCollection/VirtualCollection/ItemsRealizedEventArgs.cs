using System;

namespace VirtualCollection.VirtualCollection
{
    public class ItemsRealizedEventArgs : EventArgs
    {
        public ItemsRealizedEventArgs(int startIndex, int count)
        {
            StartingIndex = startIndex;
            Count = count;
        }

        public int StartingIndex { get; private set; }
        public int Count { get; private set; }
    }
}