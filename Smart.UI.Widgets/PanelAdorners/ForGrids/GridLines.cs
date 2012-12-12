using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    /// <summary>
    /// Class needed for drawing Gridlines
    /// </summary>
    public class GridLines : CanvasAdorner<SmartGrid>, IAdorner<SmartGrid>
    {
        /// <summary>
        /// High Z index is needed to show grid above other elements
        /// </summary>
        public static int DefZIndex = 1000;

        /// <summary>
        /// OtherLineTriplets for columns
        /// </summary>
        public SmartCollection<VerticalLinesPath> Cols = new SmartCollection<VerticalLinesPath>();

        /// <summary>
        /// OtherLineTriplets for rows
        /// </summary>
        public SmartCollection<HorizontalArrow> Rows = new SmartCollection<HorizontalArrow>();

        /// <summary>
        /// Snapping distance
        /// </summary>
        public Point SnapDist = new Point(10, 10);

        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// Defines wether grid resizing grows/decreases grid size or not
        /// </summary>
        public static readonly DependencyProperty GrowProperty =
            DependencyProperty.Register("Grow", typeof (Boolean), typeof (GridLines),
                                        new PropertyMetadata(default(Boolean)));

        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register("LineThickness",
                                                                                                      typeof (double),
                                                                                                      typeof (GridLines),
                                                                                                      new PropertyMetadata
                                                                                                          (5.0));


        /// <summary>
        /// Colors of the grid lines, does not work yet
        /// </summary>
        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register("LineColor", typeof (Brush), typeof (GridLines),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Green)));

        public Boolean Grow
        {
            get { return (Boolean) GetValue(GrowProperty); }
            set { SetValue(GrowProperty, value); }
        }

        public double LineThickness
        {
            get { return (double) GetValue(LineThicknessProperty); }
            set { SetValue(LineThicknessProperty, value); }
        }

        public Brush LineColor
        {
            get { return (Brush) GetValue(LineColorProperty); }
            set
            {
                SetValue(LineColorProperty, value);
                /*
                foreach (var col in Cols)
                {
                    col.Stroke =  value;
                }
                foreach (var row in Rows)
                {
                    row.Stroke = value;
                }            
                if(Host!=null)this.Host.InvalidateMeasure();
                 */
            }
        }

        #endregion

        public override void Activate()
        {
            base.Activate();
            Host.DragEnabled = true;
            // Host.WaitBeforeDrag = new TimeSpan();
            IObservable<ObjectFly> snap =
                from m in Panel.DockingEnter
                from e in Panel.MouseMoveSubject.TakeUntil(Panel.DockingLeave)
                select m;
            snap.Chain(OnSnap);
            Host.InvalidateMeasure();
        }

        /// <summary>
        /// Handler which fires when the dragged object is in the snapping zone
        /// </summary>
        /// <param name="fly">Dragged object</param>
        protected void OnSnap(ObjectFly fly)
        {
            Panel.GetColumnRowDistance(fly.Target);
        }

        /// <summary>
        /// Adds new line to the row
        /// </summary>
        protected virtual void AddRow()
        {
            var r = new HorizontalArrow
                        {
                            Height = LineThickness,
                            StrokeThickness = 1,
                            VerticalAlignment = VerticalAlignment.Bottom /*, Fill = LineColor */
                        };
            r.SetZIndex(DefZIndex).SetDragMode(DragMode.Custom).SetElementType(ElementType.Adorner).SetDragCanvas(
                "NearestParent");
            //Host.DragManager.AddOnDrag(r, new SimpleSubject<ObjectFly>(OnRowDrag));
            r.Cursor = Cursors.SizeNS;
            Rows.Add(r);
            Host.AddChild(r);
        }


        protected virtual void RemoveRow()
        {
            Host.RemoveChild(Rows.Pop());
        }


        /// <summary>
        /// Adds new line for the column
        /// </summary>
        protected virtual void AddColumn()
        {
            var r = new VerticalLinesPath
                        {
                            Width = LineThickness,
                            StrokeThickness = 1,
                            HorizontalAlignment = HorizontalAlignment.Right /*, Fill = LineColor*/
                        };
            r.SetZIndex(DefZIndex).SetDragMode(DragMode.Free).SetElementType(ElementType.Adorner).SetDragCanvas(
                "NearestParent");
            //Host.DragManager.AddOnDrag(r, new SimpleSubject<ObjectFly>(OnColumnDrag));            
            r.Cursor = Cursors.SizeWE;
            Cols.Add(r);
            Host.AddChild(r);
        }

        protected virtual void RemoveColumn()
        {
            Host.RemoveChild(Cols.Pop());
        }

        /// <summary>
        /// Handler that fires before measurement
        /// </summary>
        /// <param name="constrains"></param>
        protected override void OnBeforeMeasurement(Size constrains)
        {
            if (!constrains.IsValid()) return;
            int lr = Panel.RowDefinitions.Count;
            while (lr < Rows.Count) RemoveRow();
            while (lr > Rows.Count) AddRow();
            int lc = Panel.ColumnDefinitions.Count;
            while (lc < Cols.Count) RemoveColumn();
            while (lc > Cols.Count) AddColumn();

            for (int i = 0; i < lr; i++)
            {
                Rows[i]
                    .SetRow(i).SetColumn(0).SetColumnSpan(lc)
                    .SetRelativeToGrid(true).SetBottom(0.0)
                    .SetRowSplitter();
            }

            for (int i = 0; i < lc; i++)
            {
                Cols[i]
                    .SetColumn(i).SetRow(0).SetRowSpan(lr)
                    .SetRelativeToGrid(true).SetRight(0.0)
                    .SetColumnSplitter();
            }
        }
    }
}