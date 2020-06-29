using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WhereIsThePiko.Utility
{
    class CommandHandler : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public CommandHandler(Action<object> execute) : this(execute, null)
        {
        }

        public CommandHandler(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }
    }
}
