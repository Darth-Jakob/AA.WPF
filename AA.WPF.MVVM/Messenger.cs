using AA.WPF.MVVM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AA.WPF.MVVM
{
    public class Messenger : IMessenger
    {
        #region Singleton

        private static readonly Lazy<Messenger> lazy = new Lazy<Messenger>(() => new Messenger());
        public static Messenger Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        #endregion

        #region Constructor

        public Messenger()
        {
            if (Application.Current != null)
            {
                _dispatcher = Application.Current.Dispatcher;
            }
        }

        #endregion

        private List<MessageInfo> _registedMessages = null;
        private Dispatcher _dispatcher = null;

        public void Received<TMessageType>(Action<TMessageType> callbackMessage) where TMessageType : IMessage
        {
            MessageInfo<TMessageType> find = FindMessage<TMessageType>();
            if (find == null)
            {
                //기등록 메세지 없음, 새로 등록함.
                MessageInfo<TMessageType> info = new MessageInfo<TMessageType>() { MessageType = typeof(TMessageType) };
                info.Subscribe(callbackMessage);
                _registedMessages.Add(info);
            }
            else
            {
                find.Subscribe(callbackMessage);
            }
        }

        public void Send<TMessageType>(TMessageType message) where TMessageType : IMessage
        {
            if (_registedMessages != null)
            {
                MessageInfo<TMessageType> find = FindMessage<TMessageType>();
                if (find != null)
                {
                    foreach (var item in find.GetInvocationList())
                    {
                        if (_dispatcher != null && !_dispatcher.CheckAccess())
                        {
                            _dispatcher.BeginInvoke(DispatcherPriority.Normal,  new Action(() => item.Invoke(message)));
                        }
                        else
                        {
                            item.Invoke(message);
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Message is not Exist");
            }
        }

        private MessageInfo<T> FindMessage<T>()
        {
            if (_registedMessages == null)
            {
                _registedMessages = new List<MessageInfo>();
            }

            return _registedMessages.FirstOrDefault(o => o.MessageType.Equals(typeof(T))) as MessageInfo<T>;
        }
    }
}
