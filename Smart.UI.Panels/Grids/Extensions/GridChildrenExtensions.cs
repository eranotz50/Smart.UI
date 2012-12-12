using System.Linq;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;

namespace Smart.UI.Panels
{
    public static class GridChildrenExtensions
    {
        #region CHILDREN IN ROWS AND COLUMNS

        public static SmartCollection<T> ChildrenInRows<T>(this IChildrenHolder source, int row, int rowSpan = 1)
            where T : FrameworkElement
        {
            return source.Children.OfType<T>().Where(e => e.IsInRows(row, rowSpan)).ToCollection();
        }

        public static SmartCollection<T> ChildrenInColumns<T>(this IChildrenHolder source, int col, int colpsan = 1)
            where T : FrameworkElement
        {
            return source.Children.OfType<T>().Where(e => e.IsInColumns(col, colpsan)).ToCollection();
        }


        public static SmartCollection<T> ChildrenInCells<T>(this IChildrenHolder source, int col, int row,
                                                              int colSpan = 1, int rowSpan = 1)
            where T : FrameworkElement
        {
            return source.Children.OfType<T>().Where(e => e.CheckInCells(col, row, colSpan, rowSpan)).ToCollection();
        }

        public static SmartCollection<T> ChildrenInCells<T>(this IChildrenHolder source, CellsRegion cells)
            where T : FrameworkElement
        {
            return
                source.Children.OfType<T>().Where(
                    e => e.CheckInCells(cells.Col, cells.Row, cells.ColSpan, cells.RowSpan)).ToCollection();
        }

        #endregion
    }
}