using System.Windows;
using System.Windows.Media;

namespace AA.WPF.Controls.Util
{
    internal static class VisualTreeHelperEX
    {
        public static T FindParentByType<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            if (parentObject is T)
            {
                return parentObject as T;
            }
            else
            {
                return FindParentByType<T>(parentObject);
            }
        }

        public static T FindVisualChildByType<T>(DependencyObject _Control) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(_Control); i++)
            {
                var child = VisualTreeHelper.GetChild(_Control, i);
                if (child is T)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByType<T>(child);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            return null;
        }
    }
}

