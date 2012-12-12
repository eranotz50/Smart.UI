using System;

namespace Smart.UI.Panels
{
    public struct LineDistance
    {
        public double Dist;
        public Boolean Found;
        public int Num;

        public LineDistance(double dist, int num = 0, Boolean found = true)
        {
            Num = num;
            Dist = dist;
            Found = found;
        }
    }

    public struct CellsRegion
    {
        public int Col;
        public int ColSpan;
        public int Row;
        public int RowSpan;

        public CellsRegion(int col, int row, int colSpan = 1, int rowSpan = 1)
        {
            Col = col;
            Row = row;
            ColSpan = colSpan;
            RowSpan = rowSpan;
        }

        /// <summary>
        /// Total number of cells
        /// </summary>
        public int Count
        {
            get { return ColSpan*RowSpan; }
        }

        /// <summary>
        /// Getter for the lowest row
        /// NAPILNIK
        /// </summary>
        public int BottomRow
        {
            get { return Row + RowSpan; }
        }


        /// <summary>
        /// Getter for the rightest column
        /// NAPILNIK
        /// </summary>
        public int RightCol
        {
            get { return Col + ColSpan; }
        }

        public Boolean HasRow(int row)
        {
            return row >= Row && row < Row + RowSpan;
        }

        public Boolean HasColumn(int col)
        {
            return col >= Col && col < Col + ColSpan;
        }


        /// <summary>
        /// Changes Col and Row of the region to fit into the grid
        /// for some internal griduse only
        /// needs clarifying
        /// NAPILNIK
        /// </summary>
        /// <param name="colCount"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public CellsRegion PlaceInsideGrid(int colCount, int rowCount)
        {
            if (colCount <= RightCol) Col = colCount > 0 ? colCount - ColSpan : 0;
            if (rowCount <= BottomRow) Row = rowCount > 0 ? rowCount - RowSpan : 0;
            return this;
        }
    }
}