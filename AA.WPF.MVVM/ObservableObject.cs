using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.MVVM
{
    /// <summary>
    /// Data Binding 가능하도록하는 부모 class
    /// </summary>
    internal class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal void SetProperty<T>(ref T prop, T value, [CallerMemberName] string name = "")
        {
            if (!EqualityComparer<T>.Default.Equals(prop, value))
            {
                prop = value;
                RaisePropertyChanged(name);
            }
        }
    }
}
