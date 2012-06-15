using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using VirtualCollection.Framework.Extensions;

namespace VirtualCollection.Framework.MVVM
{
    public class CommandHolder
    {
        Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();
 
        public ICommand GetOrCreateCommand(Expression<Func<ICommand>> commandPropertyExpression, Action executeAction)
        {
            var name = commandPropertyExpression.GetPropertyName();
            ICommand command;
            if (!_commands.TryGetValue(name, out command))
            {
                command = new ActionCommand(_ => executeAction());
                _commands.Add(name, command);
            }

            return command;
        }
    }
}
