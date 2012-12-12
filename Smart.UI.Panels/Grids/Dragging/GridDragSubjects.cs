using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Panels
{
    public abstract class GridLineDragSubject : DragSubject<ObjectFly>, IOrientable
    {
        public LineMover Mover;

        protected GridLineDragSubject()
        {
            Mover = new LineMover();
        }

        protected GridLineDragSubject(LineMover mover)
        {
            Mover = mover;
        }

        #region IOrientable Members

        public Orientation Orientation { get; set; }

        #endregion

        protected override void OnNextHandler(ObjectFly f)
        {
            if (Mover == null) return;
            int num = this.GetLineNum(f.Target);
            int span = this.GetLineSpan(f.Target);
            this.GetParentLineDefinitions(f.Target).SplitLines(num + Mover.Source, Mover.Target + num + span - 1,
                                                               this.Coord(f.ShiftPos));
        }
    }

    public class ColumnDragSubject : GridLineDragSubject
    {
        public ColumnDragSubject()
        {
            Orientation = Orientation.Horizontal;
        }
    }

    public class RowDragSubject : GridLineDragSubject
    {
        public RowDragSubject()
        {
            Orientation = Orientation.Vertical;
        }
    }

    /// <summary>
    /// Napilnik
    /// </summary>
    public class GridLinesDragSubject : DragSubject<ObjectFly>
    {
        public LineMover ColumnMover;
        public LineMover RowMover;

        public GridLinesDragSubject()
        {
            RowMover = new LineMover();
            ColumnMover = new LineMover();
        }

        public GridLinesDragSubject(LineMover colMover, LineMover rowMover)
        {
            RowMover = colMover;
            ColumnMover = rowMover;
        }


        protected override void OnNextHandler(ObjectFly f)
        {
            FrameworkElement target = f.Target;
            var grid = target.GetParent<FlexGrid>();
            if (grid == null) return;
            if (ColumnMover != null)
            {
                int column = FlexGrid.GetColumn(target);
                int columnspan = FlexGrid.GetColumnSpan(target);

                grid.ColumnDefinitions.SplitLines(column + ColumnMover.Source,
                                                  ColumnMover.Target + column + columnspan - 1, f.ShiftPos.X);
            }
            if (RowMover == null) return;
            int row = FlexGrid.GetRow(target);
            int rowspan = FlexGrid.GetRowSpan(target);
            grid.RowDefinitions.SplitLines(row + RowMover.Source, RowMover.Target + row + rowspan - 1, f.ShiftPos.Y);
        }
    }
}