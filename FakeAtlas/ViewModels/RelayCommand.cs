using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FakeAtlas.ViewModels
{
    public class RelayCommand : ICommand 
    {
        private readonly Action<object> _execute; // приватное поле для чтения делегата
        private readonly Predicate<object> _canExecute; // приватное поле для чтения делегата

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)  // публичный метод для инициализации делегата
        
        {
            if (execute == null) throw new ArgumentNullException("execute"); //условие для проверки на null

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) // публичный метод для проверки возможности выполнения команды
        
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged // публичный метод для обновления команды

        
        {
            add { CommandManager.RequerySuggested += value; } 
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter) // публичный метод для выполнения команды

        
        {
            _execute(parameter ?? "<N/A>");
        }

    }
}
