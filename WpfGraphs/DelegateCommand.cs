using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfGraphs
{
    public class DelegateCommand:ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Predicate<object> _canExecute;
        private readonly Action<object?> _execute;

        public DelegateCommand(Predicate<object?> canExecute, Action<object?> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object? parameters)
        {
            return _canExecute.Invoke(parameters);
        }

        public void Execute(object? parameters)
        {
            _execute.Invoke(parameters);
        }

        public void ChanExecuteChange()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
