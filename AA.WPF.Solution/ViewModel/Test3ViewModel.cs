using AA.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.Solution.ViewModel
{
    public class Test3ViewModel : ObservableObject
    {
        public Test3ViewModel()
        {
            Messenger.Instance.Received<TEST1Message>(OnReciveTestMessage);
            Messenger.Instance.Received<TEST2Message>(OnRecieveTEST2Message);
        }


        void OnReciveTestMessage(TEST1Message message)
        {
            ReceiveMessage = message.Value;
        }

        void OnRecieveTEST2Message(TEST2Message message)
        {
            ReceiveMessage = message.Value;
        }

        string _receiveMessage;
        public string ReceiveMessage
        {
            get { return _receiveMessage; }
            set { _receiveMessage = value; RaisePropertyChanged("ReceiveMessage"); }
        }

    }
}
