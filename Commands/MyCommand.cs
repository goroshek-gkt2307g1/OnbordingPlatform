using System;
using System.Windows.Input;

namespace OnbordingPlatform.Commands
{
    public class MyCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public MyCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute();
    }
}