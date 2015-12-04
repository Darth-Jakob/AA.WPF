using AA.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AA.WPF.Solution
{
    public class MainVIewModel : ObservableObject
    {
        #region Command
        
        ICommand _ButtonCommand = null;
        public ICommand ButtonCommand
        {
            get
            {
                if (_ButtonCommand == null)
                {
                    _ButtonCommand = new DelegateCommand(OnExecute_ButtonCommand);
                }
                return _ButtonCommand;
            }
            set { _ButtonCommand = value; }
        }

        private void OnExecute_ButtonCommand()
        {
            MessageBox.Show("Call DelegateCommand Execute!!");
        }

        ICommand _ButtonParameterCommand = null;
        public ICommand ButtonParameterCommand
        {

            get
            {
                if (_ButtonParameterCommand == null)
                {
                    _ButtonParameterCommand = new DelegateCommand<object>(OnExecute_ButtonParameterCommand);
                }
                return _ButtonParameterCommand;
            }
            set { _ButtonParameterCommand = value; }
        }

        private void OnExecute_ButtonParameterCommand(object parameter)
        {
            MessageBox.Show(string.Format("Call DelegateCommand<T> Execute!! parameter : {0}", parameter));
        }

        #endregion

        #region DataBinding Property

        string _testText = string.Empty;

        public string TestText
        {
            get { return _testText; }
            set { _testText = value; RaisePropertyChanged("TestText"); }
        }

        #endregion
    }
}
