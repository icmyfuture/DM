using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Carrier.ViewModel
{
    /// <summary>
    ///   COMMAND的加载
    /// </summary>
    public sealed class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        ///   单命令构造
        /// </summary>
        /// <param name = "execute">命令体</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        ///   加验证构造
        /// </summary>
        /// <param name = "execute">命令体</param>
        /// <param name = "canExecute">验证方法</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        #region ICommand Members

        /// <summary>
        ///   是否允许执行的验证
        /// </summary>
        /// <param name = "parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        ///   执行
        /// </summary>
        /// <param name = "parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
}