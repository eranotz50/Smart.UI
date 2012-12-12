using System.Windows;
using Smart.UI.Panels;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    public static class SmarGridExtensions
    {
        #region  ADD ROW REMOVE LAST ROW

        /// <summary>
        /// Row insertion with reposing all children element
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="rowLen"></param>
        /// <returns></returns>
        public static T AddRow<T>(this T source, double rowLen) where T : FlexGrid
        {
            return source.AddRow(new LineDefinition(rowLen));
        }

        public static T AddRow<T>(this T source, RelativeLength rowLen) where T : FlexGrid
        {
            return source.AddRow(new LineDefinition(rowLen));
        }

        public static T AddRow<T>(this T source) where T : FlexGrid
        {
            return source.AddRow(new LineDefinition(1, 0));
        }

        /// <summary>
        /// Adds raw to the end ot the grid
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="ld"></param>
        /// <returns></returns>
        public static T AddRow<T>(this T source, LineDefinition ld) where T : FlexGrid
        {
            source.RowDefinitions.Add(ld);
            return source;
        }

        public static T AddColumn<T>(this T source, double colLen) where T : FlexGrid
        {
            return source.AddColumn(new LineDefinition(colLen));
        }

        public static T AddColumn<T>(this T source) where T : FlexGrid
        {
            return source.AddColumn(new LineDefinition(1, 0));
        }


        public static T AddColumn<T>(this T source, LineDefinition ld) where T : FlexGrid
        {
            source.ColumnDefinitions.Add(ld);
            return source;
        }

        public static T RemoveLastRow<T>(this T source) where T : FlexGrid
        {
            int num = source.RowDefinitions.Count - 1;
            source.RowDefinitions.RemoveAt(num);
            foreach (FrameworkElement child in source.ChildrenInRows<FrameworkElement>(num)) source.RemoveChild(child);
            return source;
        }

        public static T RemoveLastColumn<T>(this T source) where T : FlexGrid
        {
            int num = source.ColumnDefinitions.Count - 1;
            source.ColumnDefinitions.RemoveAt(num);
            foreach (FrameworkElement child in source.ChildrenInColumns<FrameworkElement>(num))
                source.RemoveChild(child);
            return source;
        }

        #endregion

        #region Row and columns insertions and removements

        /// <summary>
        /// Row insertion with reposing all children element
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="num"></param>
        /// <param name="rowLen"></param>
        /// <returns></returns>
        public static T InsertRow<T>(this T source, int num, double rowLen = 0.0) where T : FlexGrid
        {
            return source.InsertRow(num, new LineDefinition(rowLen));
        }


        public static T InsertRow<T>(this T source, int num, LineDefinition ld) where T : FlexGrid
        {
            source.RowDefinitions.Insert(num, ld);
            foreach (FrameworkElement child in source.Children)
            {
                int row = FlexGrid.GetRow(child);
                int span = FlexGrid.GetRowSpan(child);
                if (row >= num)
                    child.SetRow(row + 1);
                else if (row + span >= num)
                    child.SetRowSpan(span + 1);
            }

            return source;
        }

        public static T InsertColumn<T>(this T source, int num, double colLen = 0.0) where T : FlexGrid
        {
            return source.InsertColumn(num, new LineDefinition(colLen));
        }


        public static T InsertColumn<T>(this T source, int num, LineDefinition ld) where T : FlexGrid
        {
            source.ColumnDefinitions.Insert(num, ld);
            // ld.Num = num;
            foreach (FrameworkElement child in source.Children)
            {
                int col = FlexGrid.GetColumn(child);
                int span = FlexGrid.GetColumnSpan(child);
                if (col >= num)
                    child.SetColumn(col + 1);
                else if (col + span >= num)
                    child.SetColumnSpan(span + 1);
            }

            return source;
        }

        public static T RemoveRow<T>(this T source, int num) where T : FlexGrid
        {
            source.RowDefinitions.RemoveAt(num);
            foreach (FrameworkElement child in source.Children)
            {
                int row = FlexGrid.GetRow(child);
                int span = FlexGrid.GetRowSpan(child);
                if (row >= num)
                    child.SetRow(row - 1);
                else if (row + span >= num)
                    child.SetRowSpan(span - 1);
            }

            return source;
        }

        public static T RemoveColumn<T>(this T source, int num) where T : FlexGrid
        {
            source.ColumnDefinitions.RemoveAt(num);
            foreach (FrameworkElement child in source.Children)
            {
                int col = FlexGrid.GetColumn(child);
                int span = FlexGrid.GetColumnSpan(child);
                if (col >= num)
                    child.SetColumn(col - 1);
                else if (col + span >= num)
                    child.SetColumnSpan(span - 1);
            }

            return source;
        }

        #endregion
    }
}