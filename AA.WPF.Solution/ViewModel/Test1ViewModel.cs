using AA.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AA.WPF.Solution.ViewModel
{
    public class Test1ViewModel : ObservableObject
    {
        public Test1ViewModel()
        {
            Messenger.Instance.Received<TEST1Message>(OnReciveTestMessage);
        }

        ICommand _test1ButtonCommand = null;
        public ICommand Test1ButtonCommand
        {
            get
            {
                if (_test1ButtonCommand == null)
                {
                    _test1ButtonCommand = new DelegateCommand(OnExecute_Test1ButtonCommand);
                }
                return _test1ButtonCommand;
            }
            set { _test1ButtonCommand = value; }
        }


        void OnExecute_Test1ButtonCommand()
        {
            Messenger.Instance.Send<TEST1Message>(new TEST1Message() { Value = "Send from Test 1 Viewmodel" });
        }

        void OnReciveTestMessage(TEST1Message test)
        {
            System.Windows.MessageBox.Show(test.Value);
        }
    }
}
