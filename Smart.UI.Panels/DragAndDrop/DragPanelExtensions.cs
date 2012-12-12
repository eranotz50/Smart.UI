using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Panels
{
    public static class DragPanelExtensions
    {
        #region HIERARCHY RELATED

        /// <summary>
        /// вычисление самого высокого перента, способного к драгэнддропу, именно он будет использован для передвижения контролзов
        /// Временно не использую, хотя и затестил
        /// </summary>
        public static T GetHighestDragParent<T>(this FrameworkElement element) where T : DragPanel
        {
            T host = element is T ? (element as T) : null;
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                f = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (f is T) host = f as T;
            }
            return host;
        }


        /// <summary>
        /// Gets nearest dragpanel with dragenabled == true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T GetNearestDragEnabledParent<T>(this FrameworkElement element) where T : DragPanel
        {
            //var host = element is T ? (element as T) : null;            
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                f = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                var ret = f as T;
                if (ret != null && ret.DragEnabled) return ret;
            }
            return null;
            //return host;
        }

        public static DragPanel GetNearestDragParent(this FrameworkElement element)
        {
            return element.GetParent<DragPanel>();
        }

        public static T GetNearestDragParent<T>(this FrameworkElement element) where T : DragPanel
        {
            return element.GetParent<T>();
        }

        public static T GetUserControlDragParent<T>(this FrameworkElement element) where T : DragPanel
        {
            //var host = element is T ? (element as T) : null;            
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                var parent = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (f is UserControl) return f.GetNearestDragParent<T>();
                f = parent;
            }
            return null;
            //return host;
        }

        public static FrameworkElement GetNearestDragPanelChild<T>(this FrameworkElement element) where T : DragPanel
        {
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                var parent = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (parent is T) return f;
                f = parent;
            }
            return null; //f;
        }

        public static FrameworkElement GetNearestDragEnabledChild<T>(this FrameworkElement element) where T : DragPanel
        {
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                var parent = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                var ret = parent as T;
                if (ret != null && ret.DragEnabled) return f;
                f = parent;
            }
            return null; //f;
        }

        #endregion

        public static Rect GetBoundsTop<T>(this T element) where T : DragPanel
        {
            //Contract.Requires(element.Parent != null);
            //Contract.Requires(element.Parent is UIElement);
            return element.GetRelativeRect(element.GetHighestDragParent<T>());
        }
    }
}