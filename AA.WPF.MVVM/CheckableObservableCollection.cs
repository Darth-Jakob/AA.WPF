using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AA.WPF.MVVM
{
    /// <summary>
    /// patch group collection 사용
    /// item의 check 상태 관리 목적
    /// </summary>
    public class CheckableCollection<T> : ObservableCollection<T>, IDisposable where T : INotifyPropertyChanged
    {
        public CheckableCollection(IEnumerable<T> collection) : base(collection)
        {
            CheckedUpdate();
        }

        public CheckableCollection()
        {
            CheckedUpdate();
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
            CheckedUpdate();
        }

        protected override void SetItem(int index, T item)
        {
            T oldItem = Items[index];
            T newItem = item;
            oldItem.PropertyChanged -= Item_PropertyChanged;
            newItem.PropertyChanged += Item_PropertyChanged;
           
            base.SetItem(index, item);
            CheckedUpdate();
        }

        protected override void ClearItems()
        {
            foreach (T item in Items)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }
           
            base.ClearItems();
            CheckedUpdate();
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= Item_PropertyChanged;
            base.RemoveItem(index);
            CheckedUpdate();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            item.PropertyChanged += Item_PropertyChanged;
            CheckedUpdate();
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                CheckedUpdate();
            }
        }

        public void Dispose()
        {
            ClearItems();
        }

        List<T> _checkedList;
        public List<T> CheckedList
        {
            get { return _checkedList; }
            set
            {
                if (_checkedList != value)
                {
                    _checkedList = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CheckedList"));
                }
            }
        }

        bool _isAllCheckedt;
        public bool IsAllChecked
        {
            get { return _isAllCheckedt; }
            set
            {
                if (_isAllCheckedt != value)
                {
                    _isAllCheckedt = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsAllChecked"));
                }
            }
        }

        /// <summary>
        /// 모두체크상태 갱신
        /// </summary>
        private void CheckedUpdate()
        {
            //check상태는 CheckablePropertyAttribute 추가된 property로 판별
            CheckedList = Items.Where(o =>
            {
                var propertyInfo = o.GetType().GetProperties().FirstOrDefault(prop => 
                {
                    return prop.GetCustomAttributes().Any(attr => attr is CheckablePropertyAttribute);
                });
                if (propertyInfo.GetType() == typeof(bool))
                {
                    return (bool)propertyInfo.GetValue(propertyInfo);
                }
                return false;
            }).ToList();

            IsAllChecked = Items.Count == CheckedList.Count;
            if (Items.Count == 0)
            {
                IsAllChecked = false;
            }
        }
    }

    public class CheckablePropertyAttribute : Attribute
    {

    }
}
