using AA.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AA.WPF.Solution.ViewModel
{
    public class Test2ViewModel : ObservableObject
    {
        public Test2ViewModel()
        {
            Messenger.Instance.Received<TEST2Message>(OnReciveTestMessage);
        }

        ICommand _test2ButtonCommand = null;
        public ICommand Test2ButtonCommand
        {
            get
            {
                if (_test2ButtonCommand == null)
                {
                    _test2ButtonCommand = new DelegateCommand(OnExecute_Test2ButtonCommand);
                }
                return _test2ButtonCommand;
            }
            set { _test2ButtonCommand = value; }
        }

        void OnExecute_Test2ButtonCommand()
        {
            Messenger.Instance.Send<TEST2Message>(new TEST2Message() { Value = "Send from Test 2 Viewmodel" });
        }

        void OnReciveTestMessage(TEST2Message test)
        {
            System.Windows.MessageBox.Show(test.Value);
        }
    }
}
