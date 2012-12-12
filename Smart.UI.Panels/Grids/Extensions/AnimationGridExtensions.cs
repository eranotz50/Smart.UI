using System;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;

namespace Smart.UI.Panels
{
    public static class AnimationGridExtensions
    {
        #region SmartGridExtensions

        public static Animation ResizeMyRowTo(this FrameworkElement source, double newLength,
                                              LineGrowthMode growGrid = LineGrowthMode.WithPanel,
                                              TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            var parent = source.Parent as SmartGrid;
            if (parent == null) throw new Exception("Parent of this element is not FlexGrid");
            return parent.ResizeRowTo(source.GetRow(), newLength, growGrid, howLong, easing);
        }


        public static Animation ResizeMyColumnTo(this FrameworkElement source, double newLength,
                                                 LineGrowthMode growGrid = LineGrowthMode.WithPanel,
                                                 TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            var parent = source.Parent as SmartGrid;
            if (parent == null) throw new Exception("Parent of this element is not FlexGrid");
            return parent.ResizeColumnTo(source.GetColumn(), newLength, growGrid, howLong, easing);
        }


        /// <summary>
        /// Сразу и рядок и колонку ресайзим
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newLength"></param>
        /// <param name="growGrid"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation ResizeMyCellTo(this FrameworkElement source, Size newLength,
                                               LineGrowthMode growGrid = LineGrowthMode.WithPanel,
                                               TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            var parent = source.Parent as SmartGrid;
            if (parent == null) throw new Exception("Parent of this element is not FlexGrid");
            return parent.ResizeCellTo(source.GetColumn(), source.GetRow(), newLength, growGrid, howLong, easing);
        }

        public static Animation GrowWithPadding(this FrameworkElement source, Thickness padding,
                                                TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            throw new NotImplementedException("GrowWithPadding is not implemented");
        }

        public static Animation MoveToCell(this FrameworkElement element, int col, int row, int colspan = 1,
                                           int rowspan = 1, TimeSpan howLong = default(TimeSpan),
                                           IEasingFunction easing = null)
        {
            var p = element.Parent as SmartGrid;
            return p == null ? null : p.MoveToCell(element, col, row, colspan, rowspan, howLong);
        }

        #endregion
    }
}