using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AA.WPF.Controls
{
    /// <summary>
    /// 선택가능한 combbobox control
    /// </summary>
    public class MultiSelectComboBox : ComboBox
    {
        private static readonly DependencyProperty CheckedItemsProperty = DependencyProperty.Register("CheckedItems", typeof(List<object>), typeof(MultiSelectComboBox));

        public List<object> CheckedItems
        {
            get { return (List<object>)GetValue(CheckedItemsProperty); }
            set { SetValue(CheckedItemsProperty, value); }
        }

        static MultiSelectComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(typeof(MultiSelectComboBox)));
        }

        public MultiSelectComboBox()
        {
            Loaded +=  (s, e) => ChildCheckedChanged();
        }


        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MultiSelectComboBoxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is MultiSelectComboBoxItem;
        }

        internal void ChildCheckedChanged()
        {
            if (CheckedItems == null)
            {
                CheckedItems = new List<object>();
            }
            List<object> temp = null;
            StringBuilder sb = new StringBuilder();
            foreach (MultiSelectComboBoxItem item in Items)
            {
                if (item.IsChecked)
                {
                    if (temp == null)
                    {
                        temp = new List<object>();
                        sb.Append(item.Content);
                    }
                    else
                    {
                        sb.Append($",{item.Content}");
                    }
                    temp.Add(item);
                }
            }
            Text = sb.ToString();
            CheckedItems = temp;
        }
    }

    public class MultiSelectComboBoxItem : ComboBoxItem
    {
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(MultiSelectComboBoxItem), new PropertyMetadata(new PropertyChangedCallback(OnChangedIsChecked)));

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        private static void OnChangedIsChecked(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiSelectComboBoxItem control = d as MultiSelectComboBoxItem;
            //Checked 변경시 부모 combo에 
            control.ChangedChecked((bool)e.NewValue);
            
        }

        private void ChangedChecked(bool newValue)
        {
            var owner = ItemsControl.ItemsControlFromItemContainer(this);
            if (owner is MultiSelectComboBox)
            {
                ((MultiSelectComboBox)owner).ChildCheckedChanged();
            }

        }
    }
}
