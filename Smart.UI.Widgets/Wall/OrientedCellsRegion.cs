using System.Windows.Controls;
using Smart.UI.Panels;

namespace Smart.UI.Widgets
{
    public struct OrientedCellsRegion
    {
        public int Num;
        public int OtherNum;
        public int OtherSpan;
        public int Span;

        public OrientedCellsRegion(int num, int otherNum, int span = 1, int otherSpan = 1)
        {
            Num = num;
            OtherNum = otherNum;
            Span = span;
            OtherSpan = otherSpan;
        }


        public CellsRegion ToCellsRegion(Orientation orientation)
        {
            return orientation == Orientation.Horizontal
                       ? new CellsRegion(Num, OtherNum, Span, OtherSpan)
                       : new CellsRegion(OtherNum, Num, OtherSpan, Span);
        }

        public static OrientedCellsRegion FromRegion(CellsRegion region, Orientation orientation)
        {
            return orientation == Orientation.Horizontal
                       ? new OrientedCellsRegion(region.Col, region.Row, region.ColSpan, region.RowSpan)
                       : new OrientedCellsRegion(region.Row, region.Col, region.RowSpan, region.ColSpan);
        }
    }
}