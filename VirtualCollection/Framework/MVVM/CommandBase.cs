using System;
using System.Windows.Input;

namespace VirtualCollection.Framework.MVVM
{
    public class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public CommandBase()
        {
        }

        public virtual void Execute(object parameter)
        {
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        protected void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, e);
        }
    }
}
