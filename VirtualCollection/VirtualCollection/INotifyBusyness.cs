using System;

namespace VirtualCollection.VirtualCollection
{
    public interface INotifyBusyness
    {
        event EventHandler<EventArgs> IsBusyChanged;
        bool IsBusy { get; }
    }
}
