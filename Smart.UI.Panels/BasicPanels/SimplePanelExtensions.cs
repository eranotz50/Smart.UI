using System.Windows;
using System.Windows.Media;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    public static class SimplePanelExtensions
    {
        #region EASY POS SETTINGS

        public static Pos3D GetPos<T>(this T element) where T : FrameworkElement
        {
            return SimplePanel.GetPos(element);
        }

        public static T SetPos<T>(this T element, Pos3D value) where T : FrameworkElement
        {
            SimplePanel.SetPos(element, value);
            return element;
        }

        public static T SetPos<T>(this T element, double x, double y) where T : FrameworkElement
        {
            SimplePanel.SetPos(element, new Pos3D(x, y));
            return element;
        }

        public static Pos3D ExtractPos(this FrameworkElement element)
        {
            Pos3D pos = SimplePanel.GetPos(element);
            if (pos.IsValid() == false || pos.NotEmpty == false)
            {
                if (element.Parent is SimplePanel)
                {
                    Rect slot = SimplePanel.GetSlot(element);
                    pos = new Pos3D(slot.X, slot.Y, 0.0);
                }
            }
            pos.NotEmpty = true;
            return pos;
        }

        #endregion

        #region EASY SLOT SETTING

        public static Rect GetSlot<T>(this T element) where T : FrameworkElement
        {
            return SimplePanel.GetSlot(element);
        }

        public static T SetSlot<T>(this T source, Rect value) where T : FrameworkElement
        {
            SimplePanel.SetSlot(source, value);
            return source;
        }

        public static T SetSlot<T>(this T source, double x, double y, double width, double height)
            where T : FrameworkElement
        {
            SimplePanel.SetSlot(source, new Rect(x, y, width, height));
            return source;
        }

        #endregion

        #region EASY PLACE SETTING

        public static Rect GetPlace<T>(this T element) where T : FrameworkElement
        {
            return SimplePanel.GetPlace(element);
        }

        public static T SetPlace<T>(this T source, Rect value) where T : FrameworkElement
        {
            SimplePanel.SetPlace(source, value);
            return source;
        }

        public static T SetPlace<T>(this T source, double x, double y, double width, double height)
            where T : FrameworkElement
        {
            SimplePanel.SetPlace(source, new Rect(x, y, width, height));
            return source;
        }

        #endregion

        #region ELEMENTTYPE SETTING
       
        public static T SetElementType<T>(this T source, ElementType value) where T : FrameworkElement
        {
            SimplePanel.SetElementType(source, value);
            return source;
        }
       
        public static ElementType GetElementType<T>(this T source) where T : FrameworkElement
        {
            return SimplePanel.GetElementType(source);
        }
       #endregion

        #region GROW FUNCTIONS

        /*
        static public T PlaceByParent<T>(this T child, Rect rect) where T :SimplePanel
        {
            return (child.Parent as SimplePanel ?? child).PlaceChild(child, rect);
        }
        */

        public static T Grow<T>(this T child, Point shift, AlignmentX hor = AlignmentX.Center,
                                AlignmentY ver = AlignmentY.Center) where T : FrameworkElement
        {
            var parent = child.Parent as SimplePanel;
            return (parent == null) ? child.AddSize(shift) : parent.GrowChild(child, shift, hor, ver);
        }

        public static T GrowHorizontal<T>(this T child, double shift, AlignmentX hor = AlignmentX.Center)
            where T : FrameworkElement
        {
            return child.Grow(new Point(shift, 0.0), hor);
        }

        public static T GrowVertical<T>(this T child, double shift, AlignmentY ver = AlignmentY.Center)
            where T : FrameworkElement
        {
            return child.Grow(new Point(0.0, shift), AlignmentX.Center, ver);
        }

        #endregion

        public static Rect GetLocation(this FrameworkElement element)
        {
            Rect rect = GetSlot(element);
            var p = element.Parent as SimplePanel;
            return p == null
                       ? rect
                       : new Rect(rect.X - p.Space.Panel.X, rect.Y - p.Space.Panel.Y, rect.Width, rect.Height);
        }
        #if SILVERLIGHT
            
        /// <summary>
        /// Make projection in order to achieve Z-depth
        /// </summary>
        /// <param name="fe"></param>
        /// <param name="z"></param>
        public static void MakeProjection(this FrameworkElement fe, double z)
        {
            var proj = fe.Projection as PlaneProjection;
            if (proj == null)
            {
                proj = new PlaneProjection();
                fe.Projection = proj;
            }
            proj.LocalOffsetZ = z;
        }
        #endif
    }
}