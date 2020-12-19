using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WFsoft.AsyncCommand.Common
{
    public sealed class CancelAsyncCommand : ICommand
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _commandExecuting;
        private Action onCancel;
        public event Action OnCancel
        {
            add { if (onCancel is null) onCancel += value; }
            remove { onCancel -= value; }
        }
        //public event Action OnCancel;
        public CancellationToken Token => _cts.Token;

        public void NotifyCommandStarting()
        {
            _commandExecuting = true;
            if (!_cts.IsCancellationRequested)
                return;
            _cts = new CancellationTokenSource();
            RaiseCanExecuteChanged();
        }

        public void NotifyCommandFinished()
        {
            _commandExecuting = false;
            RaiseCanExecuteChanged();
        }

        public bool CanExecute(object parameter) {
            //Debug.WriteLine("_commandExecuting:" + _commandExecuting);
            //Debug.WriteLine("!IsCancellationRequested:" + !_cts.IsCancellationRequested);
            return _commandExecuting && !_cts.IsCancellationRequested;
        } 

        public void Execute(object parameter)
        {

            onCancel?.Invoke();
            _cts.Cancel();
            RaiseCanExecuteChanged();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}
