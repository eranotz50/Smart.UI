using System;
using System.Windows;
using Smart.UI.Panels;

namespace Smart.UI.Panels
{
    public static class FlexGridExtensions
    {
        public static T SetRow<T>(this T source, int value = 0) where T : FrameworkElement
        {
            FlexGrid.SetRow(source, value);
            return source;
        }

        public static T SetRowSpan<T>(this T source, int value = 0) where T : FrameworkElement
        {
            FlexGrid.SetRowSpan(source, value);
            return source;
        }

        public static T SetColumn<T>(this T source, int value = 0) where T : FrameworkElement
        {
            FlexGrid.SetColumn(source, value);
            return source;
        }

        public static T SetColumnSpan<T>(this T source, int value = 0) where T : FrameworkElement
        {
            FlexGrid.SetColumnSpan(source, value);
            return source;
        }

        public static T SetRelativeToGrid<T>(this T source, Boolean value = true) where T : FrameworkElement
        {
            FlexGrid.SetRelativeToGrid(source, value);
            return source;
        }

        public static int GetRow(this FrameworkElement source)
        {
            return FlexGrid.GetRow(source);
        }

        public static int GetRowSpan(this FrameworkElement source)
        {
            return FlexGrid.GetRowSpan(source);
        }

        public static int GetColumn(this FrameworkElement source)
        {
            return FlexGrid.GetColumn(source);
        }

        public static int GetColumnSpan(this FrameworkElement source)
        {
            return FlexGrid.GetColumnSpan(source);
        }

        #region NAPILNIK RELATIVE

        static public T MakeRelative<T>(this T element) where T:FrameworkElement
        {
            return FlexGrid.GetRelativeToGrid(element) ? element : element.MakeRelative(0);
        }

        static public T  MakeRelative<T>(this T element, double left, double top =0 , double right = 0, double bottom = 0)where T:FrameworkElement
        {
            element.SetRelativeToGrid(true).SetLeft(left).SetRight(right).SetTop(top).SetBottom(bottom);
            return element;

        }

        //makeselement absoulte
        static public void MakeAbsolute(this FrameworkElement element)
        {
            if (FlexGrid.GetRelativeToGrid(element) == false) return;
            element.SetRelativeToGrid(false).SetLeft(double.NegativeInfinity).SetRight(double.NegativeInfinity).SetTop(double.NegativeInfinity).SetBottom(double.NegativeInfinity);               
        }
        #endregion

        /// <summary>
        /// gets rows definition of selected element
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static LineDefinition GetRowDefinition(this FrameworkElement source)
        {
            var parent = (source.Parent as FlexGrid);
            if (parent == null) throw new Exception("There is no FlexGrid parent for this control!");
            return parent.RowDefinitions[source.GetRow()];
        }

        public static LineDefinition GetColumnDefinition(this FrameworkElement source)
        {
            var parent = (source.Parent as FlexGrid);
            if (parent == null) throw new Exception("There is no FlexGrid parent for this control!");
            return parent.ColumnDefinitions[source.GetColumn()];
        }

        /// <summary>
        /// gets rowdefintion of the element
        /// if element has rowspan gets central row
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static LineDefinition GetCentralRowDefinition(this FrameworkElement source)
        {
            var parent = (source.Parent as FlexGrid);
            if (parent == null) throw new Exception("There is no FlexGrid parent for this control!");
            var center = (int) Math.Round(0.5*source.GetRowSpan() + 0.1);
            return parent.RowDefinitions[source.GetRow() + center - 1];
        }


        public static LineDefinition GetCentralColumnDefinition(this FrameworkElement source)
        {
            var parent = (source.Parent as FlexGrid);
            if (parent == null) throw new Exception("There is no FlexGrid parent for this control!");
            var center = (int) Math.Round(0.5*source.GetColumnSpan() + 0.1);
            return parent.ColumnDefinitions[source.GetColumn() + center - 1];
        }

        #region SELECTORS

        /// <summary>
        /// Checks whether element is fully or partially inside rows interval
        /// </summary>
        /// <param name="element"></param>
        /// <param name="row">starting row</param>
        /// <param name="rowSpan">how many rows effected</param>
        /// <returns></returns>
        public static bool IsInRows(this FrameworkElement element, int row, int rowSpan = 1)
        {
            int r = GetRow(element);
            int rSpan = GetRowSpan(element);
            bool vert = (r < row + rowSpan && r + rSpan > row);
            return vert;
        }


        /// <summary>
        /// Checks whether element is fully or partially inside columns interval
        /// </summary>
        /// <param name="element"></param>
        /// <param name="col">starting columns</param>
        /// <param name="colSpan">how many column effected</param>
        /// <returns></returns>
        public static bool IsInColumns(this FrameworkElement element, int col, int colSpan = 1)
        {
            int c = GetColumn(element);
            int cSpan = GetColumnSpan(element);
            bool hor = (c < col + colSpan && c + cSpan > col);
            return hor;
        }

        /// <summary>
        /// checks whether the item falls into the cells interval
        /// </summary>
        /// <param name="element">children</param>
        /// <param name="col">column</param>
        /// <param name="row">row</param>
        /// <param name="colSpan">colspan</param>
        /// <param name="rowSpan">rowSpan</param>
        /// <returns></returns>
        public static bool CheckInCells(this FrameworkElement element, int col, int row, int colSpan = 1,
                                        int rowSpan = 1)
        {
            return element.IsInColumns(col, colSpan) && element.IsInRows(row, rowSpan);
        }

        #endregion
    }
}