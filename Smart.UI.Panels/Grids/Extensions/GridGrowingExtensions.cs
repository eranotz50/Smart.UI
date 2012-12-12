using Smart.UI.Panels;

namespace Smart.UI.Panels
{
    public static class GridGrowingExtensions
    {
        #region GROW COLUMNS AND ROWS

        public static T GrowRow<T>(this T source, int row, double shift,
                                   LineGrowthMode grow = LineGrowthMode.WithRightNeighbour) where T : FlexGrid
        {
            source.RowDefinitions.GrowLine(row, shift, grow);
            source.InvalidateMeasure();
            return source;
        }

        public static T ResizeRow<T>(this T source, int row, double newLength,
                                     LineGrowthMode grow = LineGrowthMode.WithRightNeighbour) where T : FlexGrid
        {
            return source.GrowRow(row, newLength - source.RowDefinitions[row].Value, grow);
        }

        public static T GrowColumn<T>(this T source, int col, double shift,
                                      LineGrowthMode growGrid = LineGrowthMode.WithRightNeighbour) where T : FlexGrid
        {
            source.ColumnDefinitions.GrowLine(col, shift, growGrid);
            source.InvalidateMeasure();
            return source;
        }

        public static T ResizeColumn<T>(this T source, int col, double newLength,
                                        LineGrowthMode growGrid = LineGrowthMode.WithRightNeighbour) where T : FlexGrid
        {
            return source.GrowColumn(col, newLength - source.ColumnDefinitions[col].Value, growGrid);
        }

        #endregion
    }
}