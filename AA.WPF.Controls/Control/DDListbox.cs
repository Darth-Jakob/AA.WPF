using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AA.WPF.Controls
{
    /// <summary>
    /// drag & drop 구현한 listbox
    /// </summary>
    public class DDListbox : ListBox
    {
        public static RoutedEvent DropItemMoveEvent = EventManager.RegisterRoutedEvent("DropItemMove", RoutingStrategy.Bubble, typeof(DropItemMoveEventArgs), typeof(DDListbox));
        /// <summary>
        /// drag item 순서 변경 이벤트
        /// </summary>
        public event RoutedEventHandler DropItemMove
        {
            add { AddHandler(DropItemMoveEvent, value); }
            remove { RemoveHandler(DropItemMoveEvent, value); }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DDListboxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DDListboxItem;
        }

        /// <summary>
        /// drop으로 item move
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dDListboxItem"></param>
        internal void DropMoveItem(DDListboxItem source, DDListboxItem target)
        {
            //이벤트 발생
            RaiseEvent(new DropItemMoveEventArgs(DropItemMoveEvent) { DargSource = source, DropTarget = target });

            //ItemsSource 사용안할시 내부에서 직접 순서 변경함. 
            if (ItemsSource == null)
            {
                int idx = ItemContainerGenerator.IndexFromContainer(target);
                var sourceData = ItemContainerGenerator.ItemFromContainer(source);
                Items.Remove(sourceData);
                Items.Insert(idx, sourceData);
            }
        }
    }

    /// <summary>
    /// drag 순서 변경 RoutedEventArgs
    /// </summary>
    public class DropItemMoveEventArgs : RoutedEventArgs
    {
        public DropItemMoveEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        /// <summary>
        /// drag object
        /// </summary>
        internal object DargSource { get; set; }
        /// <summary>
        /// drop target
        /// </summary>
        internal object DropTarget { get; set; }
    }
    /// <summary>
    /// drag & drop 구현한 listboxItem
    /// </summary>
    public class DDListboxItem : ListBoxItem
    {
        public static readonly DependencyProperty IsDragOverProperty = DependencyProperty.Register("IsDragOver", typeof(bool), typeof(DDListboxItem));
        public bool IsDragOver
        {
            get { return (bool)GetValue(IsDragOverProperty); }
            set { SetValue(IsDragOverProperty, value); }
        }


        Point _moveMousePoint;
        Point _startMousePoint;

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                _startMousePoint = e.GetPosition(this);
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //마우스 다운이 되지않아 일정 간격 이동시에 drag로 간주한다. 일단 15픽셀 이동시
                _moveMousePoint = e.GetPosition(this);
                if (Math.Abs(_startMousePoint.X - _moveMousePoint.X) > 15 || Math.Abs(_startMousePoint.Y - _moveMousePoint.Y) > 15)
                {
                    //drag 시작
                    DataObject dragData = new DataObject("ChannelItem", this);
                    DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
                }
            }
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            IsDragOver = false;
            base.OnPreviewDrop(e);

            //dragdata 검증
            if (e.Data.GetDataPresent("ChannelItem"))
            {
                var dragSource = e.Data.GetData("ChannelItem");

                var owner = ItemsControl.ItemsControlFromItemContainer(this);
                if (owner is DDListbox)
                {
                    //부모 itemcontrol 함수 호출
                    ((DDListbox)owner).DropMoveItem(dragSource as DDListboxItem, this);
                }
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent("ChannelItem"))
            {
                IsDragOver = true;
            }
        }

        protected override void OnDragLeave(DragEventArgs e)
        {
            IsDragOver = false;
            base.OnDragLeave(e);
        }
    }
}
