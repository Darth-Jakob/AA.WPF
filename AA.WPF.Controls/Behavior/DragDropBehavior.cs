using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using AA.WPF.Controls.Util;

namespace AA.WPF.Controls.Behavior
{
    public class DragDropBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.Register("AllowDrag", typeof(bool), typeof(DragDropBehavior));

        public static readonly DependencyProperty AllowDropProperty = DependencyProperty.Register("AllowDrop", typeof(bool), typeof(DragDropBehavior));

        public static readonly DependencyProperty DragIDProperty = DependencyProperty.Register("DragID", typeof(string), typeof(DragDropBehavior));

        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(DragDropBehavior));

        public ICommand DropCommand
        {
            get { return (ICommand)GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public bool AllowDrag
        {
            get { return (bool)GetValue(AllowDragProperty); }
            set { SetValue(AllowDragProperty, value); }
        }

        public bool AllowDrop
        {
            get { return (bool)GetValue(AllowDropProperty); }
            set { SetValue(AllowDropProperty, value); }
        }

        public string DragID
        {
            get { return GetValue(DragIDProperty) as string; }
            set { SetValue(DragIDProperty, value); }
        }

        Point _moveMousePoint;
        Point _startMousePoint;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AllowDrag)
            {
                AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
                AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            }

            if (AllowDrop)
            {
                AssociatedObject.AllowDrop = true;
                AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;
                AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;
            }
        }


        protected override void OnDetaching()
        {
            if (AllowDrag)
            {
                AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
                AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            }

            if (AllowDrop)
            {
                AssociatedObject.PreviewDrop -= AssociatedObject_PreviewDrop;
            }
            base.OnDetaching();
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //drag 시작
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _moveMousePoint = e.GetPosition(AssociatedObject);
                if (Math.Abs(_startMousePoint.X - _moveMousePoint.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(_startMousePoint.Y - _moveMousePoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DataObject dragData = null;
                    if (e.Source is ListBox)
                    {
                        var lbi = VisualTreeHelperEX.FindParentByType<ListBoxItem>((DependencyObject)e.OriginalSource);
                        dragData = new DataObject(DragID, lbi);
                    }
                    else
                    {
                        dragData = new DataObject(DragID, e.Source);
                    }
                    DragDrop.DoDragDrop(AssociatedObject, dragData, DragDropEffects.Move);
                }
            }
        }

        private void AssociatedObject_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                _startMousePoint = e.GetPosition(AssociatedObject);
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            //taget drag 검사
            if (!e.Data.GetDataPresent(DragID))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            if (e.AllowedEffects == DragDropEffects.None)
            {
                e.Handled = true;
                return;
            }

            if (DropCommand != null)
            {
                DropCommand.Execute(e.Data.GetData(DragID));
            }

            e.Handled = true;
        }
    }
}
