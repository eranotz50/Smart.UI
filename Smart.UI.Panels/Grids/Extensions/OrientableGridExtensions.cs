using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Panels
{
    public static class OrientableGridExtensions
    {
        #region LINE NUMS

        public static T SetLineNum<T>(this T source, FrameworkElement element, int value) where T : IOrientable
        {
            if (source.Orientation == Orientation.Horizontal)
                FlexGrid.SetColumn(element, value);
            else
                FlexGrid.SetRow(element, value);
            return source;
        }

        public static T SetLine<T>(this T source, FrameworkElement element, LineDefinition ld) where T : IOrientable
        {
            return source.SetLineNum(element, ld.Num);
        }

        public static T SetLineNum<T>(this T source, int value) where T : FrameworkElement, IOrientable
        {
            return source.SetLineNum(source, value);
        }


        public static int GetLineNum<T>(this T source, FrameworkElement element) where T : IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? FlexGrid.GetColumn(element) : FlexGrid.GetRow(element);
        }

        public static int GetLineNum<T>(this T source) where T : FrameworkElement, IOrientable
        {
            return source.GetLineNum(source);
        }

        public static T SetLineSpan<T>(this T source, FrameworkElement element, int value) where T : IOrientable
        {
            if (source.Orientation == Orientation.Horizontal)
                FlexGrid.SetColumnSpan(element, value);
            else
                FlexGrid.SetRowSpan(element, value);
            return source;
        }


        public static T SetLineSpan<T>(this T source, int value) where T : FrameworkElement, IOrientable
        {
            return source.SetLineSpan(source, value);
        }


        public static int GetLineSpan<T>(this T source, FrameworkElement element) where T : IOrientable
        {
            return source.Orientation == Orientation.Horizontal
                       ? FlexGrid.GetColumnSpan(element)
                       : FlexGrid.GetRowSpan(element);
        }

        public static int GetLineSpan<T>(this T source) where T : FrameworkElement, IOrientable
        {
            return source.GetLineNum(source);
        }

        #endregion

        #region OTHERLINE NUMS

        public static T SetOtherLineNum<T>(this T source, FrameworkElement element, int value) where T : IOrientable
        {
            if (source.Orientation == Orientation.Vertical)
                FlexGrid.SetColumn(element, value);
            else
                FlexGrid.SetRow(element, value);
            return source;
        }

        public static T SetOtherLineNum<T>(this T source, int value) where T : FrameworkElement, IOrientable
        {
            return source.SetOtherLineNum(source, value);
        }

        public static T SetOtherLineSpan<T>(this T source, FrameworkElement element, int value) where T : IOrientable
        {
            if (source.Orientation == Orientation.Vertical)
                FlexGrid.SetColumnSpan(element, value);
            else
                FlexGrid.SetRowSpan(element, value);
            return source;
        }

        public static T SetOtherLineSpan<T>(this T source, int value) where T : FrameworkElement, IOrientable
        {
            return source.SetOtherLineSpan(source, value);
        }

        public static int GetOtherLineNum<T>(this T source, FrameworkElement element) where T : IOrientable
        {
            return source.Orientation == Orientation.Vertical ? FlexGrid.GetColumn(element) : FlexGrid.GetRow(element);
        }


        public static int GetOtherLineNum<T>(this T source) where T : FrameworkElement, IOrientable
        {
            return source.GetOtherLineNum(source);
        }

        public static int GetOtherLineSpan<T>(this T source, FrameworkElement element) where T : IOrientable
        {
            return source.Orientation == Orientation.Vertical
                       ? FlexGrid.GetColumnSpan(element)
                       : FlexGrid.GetRowSpan(element);
        }

        public static int GetOtherLineSpan<T>(this T source) where T : FrameworkElement, IOrientable
        {
            return source.GetOtherLineNum(source);
        }

        #endregion

        #region LINE DEFINITIONS

        public static LineDefinitions GetParentLineDefinitions<T>(this T source, FrameworkElement element)
            where T : IOrientable
        {
            var grid = element.GetParent<FlexGrid>();
            return source.Orientation == Orientation.Horizontal ? grid.ColumnDefinitions : grid.RowDefinitions;
        }

        public static LineDefinitions GetParentLineDefinitions<T>(this T source) where T : FrameworkElement, IOrientable
        {
            return source.GetParentLineDefinitions(source);
        }

        #endregion

        #region OTHERLINE DEFINITIONS

        public static LineDefinitions GetParentOtherLineDefinitions<T>(this T source, FrameworkElement element)
            where T : IOrientable
        {
            var grid = element.GetParent<FlexGrid>();
            return source.Orientation == Orientation.Vertical ? grid.ColumnDefinitions : grid.RowDefinitions;
        }

        public static LineDefinitions GetParentOtherLineDefinitions<T>(this T source)
            where T : FrameworkElement, IOrientable
        {
            return source.GetParentLineDefinitions(source);
        }

        #endregion

        #region ADD LINES

        public static T AddLine<T>(this T source) where T : SmartGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.AddColumn() : source.AddRow();
        }

        public static T AddOtherLine<T>(this T source) where T : FlexGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.AddRow() : source.AddColumn();
        }

        public static T AddLine<T>(this T source, LineDefinition ld) where T : FlexGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.AddColumn(ld) : source.AddRow(ld);
        }

        public static T AddOtherLine<T>(this T source, LineDefinition ld) where T : FlexGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.AddRow(ld) : source.AddColumn(ld);
        }

        public static T RemoveLastLine<T>(this T source) where T : FlexGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.RemoveLastColumn() : source.RemoveLastRow();
        }

        public static T RemoveLastOtherLine<T>(this T source) where T : FlexGrid, IOrientable
        {
            return source.Orientation == Orientation.Horizontal ? source.RemoveLastRow() : source.RemoveLastColumn();
        }

        #endregion

        public static LineDefinition WriteLineNum(this LineDefinition source, FrameworkElement element,
                                                  Orientation orientation = Orientation.Horizontal)
        {
            if (orientation == Orientation.Horizontal) element.SetColumn(source.Num);
            else element.SetRow(source.Num);
            return source;
        }
    }
}