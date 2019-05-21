using AA.WPF.MVVM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.MVVM
{
    internal class MessageInfo<TMessageType> : MessageInfo
    {
        internal override Type MessageType { get; set; }

        internal override object Value { get; set; }

        internal IList<Action<TMessageType>> messageActions = new List<Action<TMessageType>>();

        internal void Subscribe(Action<TMessageType> callbackMessage)
        {
            ////기등록된 delegate 있는지 검사
            //if (messageActions.Any(o => o.Method.Name == callbackMessage.Method.Name))
            //{
            //    throw new Exception("already Delegte Method subscribed");
            //}
            messageActions.Add(callbackMessage);
        }

        internal IList<Action<TMessageType>> GetInvocationList()
        {
            return messageActions;
        }
    }

    internal abstract class MessageInfo
    {
        internal abstract Type MessageType { get; set; }
        internal abstract object Value { get; set; }
    }
}
