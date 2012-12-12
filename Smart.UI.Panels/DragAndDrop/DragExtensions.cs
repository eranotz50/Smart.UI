using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.Classes.Subjects;

namespace Smart.UI.Panels
{
    public static class DragExtensions
    {
        #region EASY DRAG SETTINGS

        public static T SetDragSubject<T>(this T source, SimpleSubject<ObjectFly> value) where T : FrameworkElement
        {
            DragManager.SetDragSubject(source, value);
            return source;
        }

        public static T GetDragSubject<T>(this FrameworkElement source) where T : SimpleSubject<ObjectFly>
        {
            return DragManager.GetDragSubject(source) as T;
        }

        public static T SetDragMode<T>(this T source, DragMode value) where T : FrameworkElement
        {
            DragManager.SetDragSubject(source, DragSubjectConverter.Instance.ConvertFrom(value));
            return source;
        }


        public static T SetDragCanvas<T>(this T source, String value) where T : FrameworkElement
        {
            DragManager.SetDragCanvas(source, DragCanvasConverter.Instance.ConvertFrom(value));
            return source;
        }

        public static T SetDragTarget<T>(this T source, DragTargetFunc value) where T : FrameworkElement
        {
            DragManager.SetDragTarget(source, value);
            return source;
        }


        public static T SetColumnSplitter<T>(this T source) where T : FrameworkElement
        {
            int columnSpan = source.GetColumnSpan();
            GridDragManager.SetColumnMover(source, new LineMover(columnSpan - 1, 1));
            return source;
        }


        public static ObjectFly SetDockMode(this ObjectFly source, DockMode value)
        {
            source.DockMode = value;
            return source;
        }

        public static T SetRowSplitter<T>(this T source) where T : FrameworkElement
        {
            int rowSpan = source.GetRowSpan();
            GridDragManager.SetRowMover(source, new LineMover(rowSpan - 1, 1));
            return source;
        }

        public static T SetRowMover<T>(this T source, LineMover value) where T : FrameworkElement
        {
            GridDragManager.SetRowMover(source, value);
            return source;
        }

        public static T SetRowMover<T>(this T source, LineGrowthMode growthMode = LineGrowthMode.WithRightNeighbour,
                                       int src = -1, int target = 1) where T : FrameworkElement
        {
            GridDragManager.SetRowMover(source,
                                        new LineMover
                                            {
                                                Growth = growthMode,
                                                Source = src,
                                                Target = target
                                            });
            return source;
        }

        public static T SetColumnMover<T>(this T source, LineMover value) where T : FrameworkElement
        {
            GridDragManager.SetColumnMover(source, value);
            return source;
        }

        public static T SetColumnMover<T>(this T source, LineGrowthMode growthMode = LineGrowthMode.WithRightNeighbour,
                                          int src = -1, int target = 1) where T : FrameworkElement
        {
            GridDragManager.SetColumnMover(source, new LineMover
                                                       {
                                                           Growth = growthMode,
                                                           Source = src,
                                                           Target = target
                                                       });
            return source;
        }

        #region SET CHILDREN DRAG

        /*
                public static T SetChildrenDragCanvas<T>(this T source, DragCanvas value) where T : DragPanel
                { DragManager.SetChildrenDragCanvas(source, DragCanvasConverter.Instance.ConvertFrom(value)); return source; }
        */

        public static T SetChildrenDragCanvas<T>(this T source, String value) where T : DragPanel
        {
            DragManager.SetChildrenDragCanvas(source, DragCanvasConverter.Instance.ConvertFrom(value));
            return source;
        }

        public static T SetChildrenDockMode<T>(this T source, DockMode value) where T : DragPanel
        {
            DragManager.SetChildrenDockMode(source, value);
            return source;
        }

        #endregion

        #endregion

        public static T GetNearestControlParent<T>(this FrameworkElement element) where T : DragPanel
        {
            //var host = element is T ? (element as T) : null;            
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                var parent = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (f is Control) return f.GetNearestDragParent<T>();
                f = parent;
            }
            return null;
            //return host;
        }
    }
}