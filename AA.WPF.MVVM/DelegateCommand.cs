using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AA.WPF.MVVM
{
    /// <summary>
    /// DelegateCommand 공통 부모 class
    /// </summary>
    public class DelegateCommandBase : ICommand
    {
        internal Action _execute = null;
        internal Action<object> _executeWithParameter = null;
        internal Predicate<object> _canExecute = null;
        internal Dispatcher _dispatcher = null;

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        /// <summary>
        /// UI Thread에 접근 가능한 이벤트로 등록하도록 분기처리
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            EventHandler canExecuteChangedHandler = CanExecuteChanged;
            if (canExecuteChangedHandler != null)
            {
                if (_dispatcher != null && !_dispatcher.CheckAccess())
                {
                    _dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)OnCanExecuteChanged);
                }
                else
                {
                    canExecuteChangedHandler(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }

            if (_executeWithParameter != null)
            {
                _executeWithParameter(parameter);
            }
        }
    }

    public class DelegateCommand : DelegateCommandBase
    {
        /// <summary>
        /// 생성자 오버라이드
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action execute) : this(execute, null)
        {
            
        }

        public DelegateCommand(Action execute, Predicate<object> canExecute)
        {
            if (execute == null && canExecute == null)
            {
                 throw new ArgumentNullException("DelegateCommand Excute Method is null");
            }

            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
            _execute = execute;
            _canExecute = canExecute;
        }
    }

    //Generic 타입의 DelegateCommand
    public class DelegateCommand<T> : DelegateCommandBase
    {
        public DelegateCommand(Action<T> execute, Predicate<object> canExecute)
        {
            if (execute == null && canExecute == null)
            {
                throw new ArgumentNullException("DelegateCommand Excute Method is null");
            }

            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;    
            }
            _executeWithParameter = new Action<object>( o => execute((T)o));
            _canExecute = canExecute;
        }

        /// <summary>
        /// 생성자 오버라이드
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<T> execute) : this(execute, null)
        {
        }
      
        public bool CanExecute(T parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }
    
        public void Execute(T parameter)
        {
            if (_executeWithParameter != null)
            {
                _executeWithParameter((object)parameter);
            }
        }
    }
}
