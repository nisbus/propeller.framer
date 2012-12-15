using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PortScanner
{
    public class SimpleCommand : ICommand
    {
        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action ExecuteDelegate { get; set; }

        public SimpleCommand(Action execute, Predicate<object> canExecute)
        {
            this.ExecuteDelegate = execute;
            this.CanExecuteDelegate = canExecute;
        }

        #region ICommand Members
        
        public bool CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
                return CanExecuteDelegate(parameter);
            return true;// if there is no can execute default to true
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
     
        public void Execute(object parameter)
        {
            if (ExecuteDelegate != null)
                ExecuteDelegate();
        }
        #endregion
    }
}
