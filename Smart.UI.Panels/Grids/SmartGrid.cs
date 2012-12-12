using System;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Panels;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

//using System.Monads;

namespace Smart.UI.Panels
{
    public class SmartGrid : FlexGrid
    {
        public static readonly DependencyProperty SnapXProperty =
            DependencyProperty.Register("SnapX", typeof (double), typeof (SmartGrid), new PropertyMetadata(0.0));

        public static readonly DependencyProperty SnapYProperty =
            DependencyProperty.Register("SnapY", typeof (double), typeof (SmartGrid), new PropertyMetadata(0.0));
        /*
        public static readonly DependencyProperty GridMarginProperty =
            DependencyProperty.RegisterAttached("GridMargin", typeof (Thickness), typeof (FrameworkElement),
                                                new PropertyMetadata(default(Thickness), InvalidateParentMeasureCallback));
            public static Thickness GetGridMargin(FrameworkElement element)
        {
            return (Thickness) element.GetValue(GridMarginProperty);
        }
         */
        public SmartGrid()
        {
            InitSnapping();
        }


       

        #region SNAPPING

        public Animation Swim;

        protected void InitSnapping()
        {
            Swim = new Animation(new TimeSpan(0, 0, 0, 0, 400), Easings.CubicEaseInOut);
            MouseMoveSubject.Add(i => Swim.Detach());
            DockingEnter.Add(SnapToGrid);
        }

        protected void SnapToGrid(ObjectFly fly)
        {
            double x = SnapX;
            double y = SnapY;
            if (x.Equals(0.0) && y.Equals(0.0)) return;
            fly.ObjectShift = default(Point);

            Tuple<LineDistance, LineDistance> rel = GetColumnRowDistance(fly.Target /*,fly.ObjectShift.Invert()*/);
            //var rel = GetColumnRowDistance(fly.Target);
            //var move = new Animation(new TimeSpan(0,0,0,1),i=> { },null,Easings.CubicEaseInOut)
            var to = new Point(rel.Item1.Dist.Abs() <= x ? rel.Item1.Dist : 0.0,
                               rel.Item2.Dist.Abs() <= y ? rel.Item2.Dist : 0.0);
            if (to.Equals(default(Point))) return;
            Point from = fly.ObjectShift;
            Point delta = to.Substract(from);
            Point prev = fly.ObjectShift;
            Swim.DoOnNext = i =>
                                {
                                    Point val = delta.MultiplyBy(i);
                                    fly.ObjectShift = val.Substract(prev);
                                    fly.LastMouse = fly.CurrentMouse;
                                    fly.Pos();
                                    prev = val;
                                };
            Swim.Go();


            /*
            
             fly.ObjectShift.X = rel.Item1.Dist.Abs() <= x ? rel.Item1.Dist : 0.0;
             fly.ObjectShift.Y = rel.Item2.Dist.Abs() <= y ? rel.Item2.Dist : 0.0;
            */
        }

        #endregion

        #region ColumnRowDistances

        /// <summary>
        /// Returns distances from the element to the nearest column and row respectively
        /// </summary>
        /// <param name="element">element</param>
        /// <param name="shift"> shift coords</param>
        /// <returns></returns>
        public Tuple<LineDistance, LineDistance> GetColumnRowDistance(FrameworkElement element,
                                                                      Point shift = default(Point))
        {
            Rect bounds = element.GetRelativeRect(this);
            return new Tuple<LineDistance, LineDistance>(
                ColumnDefinitions.GetLineDistance(bounds.X + shift.X, bounds.Width + shift.X),
                RowDefinitions.GetLineDistance(bounds.Y + shift.Y, bounds.Height + shift.Y)
                );
        }

        /// <summary>
        ///  Returns distances from the rectangle to the nearest column and row respectively
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public Tuple<LineDistance, LineDistance> GetColumnRowDistance(Rect bounds)
        {
            return new Tuple<LineDistance, LineDistance>(
                ColumnDefinitions.GetLineDistance(bounds.X, bounds.Width),
                RowDefinitions.GetLineDistance(bounds.Y, bounds.Height)
                );
        }

        /// <summary>
        ///  Returns distances from the coord to the nearest column and row respectively
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public Tuple<LineDistance, LineDistance> GetColumnRowDistance(Point coords)
        {
            return new Tuple<LineDistance, LineDistance>(
                ColumnDefinitions.GetLineDistance(coords.X),
                RowDefinitions.GetLineDistance(coords.Y)
                );
        }

        #endregion

        #region Resize Animations

        /// <summary>
        /// Анимационно меняем ряд
        /// </summary>
        /// <param name="num">Номер рядка</param>
        /// <param name="newLength">новое значение</param>
        /// <param name="grow"> </param>
        /// <param name="howLong">продолждительность</param>
        /// <param name="easing">изинг</param>
        /// <returns></returns>
        public Animation ResizeRowTo(int num, double newLength, LineGrowthMode grow = LineGrowthMode.WithPanel,
                                     TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        LineDefinition row = RowDefinitions[num];
                        double val = row.Value;
                        double delta = newLength - row.Value;
                        ani.DoOnNext += q => this.ResizeRow(num, val + delta*q, grow);
                        ani.DoOnCompleted += () => this.ResizeRow(num, newLength, grow);
                        ani.OnNext(i);
                    });
        }

        public Animation ResizeColumnTo(int num, double newLength, LineGrowthMode grow = LineGrowthMode.WithPanel,
                                        TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        LineDefinition col = ColumnDefinitions[num];
                        double val = col.Value;
                        double delta = newLength - col.Value;
                        ani.DoOnNext += q =>
                                        this.ResizeColumn(num, val + delta*q, grow);
                        ani.DoOnCompleted += () => this.ResizeColumn(num, newLength, grow);
                        ani.OnNext(i);
                    });
        }

        /// <summary>
        /// Сразу и рядок и колонку ресайзим
        /// </summary>
        /// <param name="colNum"></param>
        /// <param name="rowNum"></param>
        /// <param name="newLength"></param>
        /// <param name="grow"> </param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public Animation ResizeCellTo(int colNum, int rowNum, Size newLength,
                                      LineGrowthMode grow = LineGrowthMode.WithPanel,
                                      TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        LineDefinition row = RowDefinitions[rowNum];
                        LineDefinition col = ColumnDefinitions[colNum];
                        var val = new Size(col.Value, row.Value);

                        var delta = new Point(newLength.Width - val.Width, newLength.Height - val.Height);
                        ani.DoOnNext += q =>
                                            {
                                                this.ResizeColumn(colNum, val.Width + delta.X*q, grow);
                                                this.ResizeRow(rowNum, val.Height + delta.Y*q, grow);
                                            };
                        ani.DoOnCompleted += () =>
                                                 {
                                                     this.ResizeColumn(colNum, newLength.Width, grow);
                                                     this.ResizeRow(rowNum, newLength.Height, grow);
                                                 };
                        ani.OnNext(i);
                    });
        }

        #endregion

        #region MOVEMENTS BETWEEN CELLS

        public Animation MoveToCell(FrameworkElement element, int col, int row, int colspan = 1, int rowspan = 1,
                                    TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        Rect from = GetPlace(element);
                        if (from.Equals(Rect.Empty)) from = element.GetBounds();
                        element.SetPos(default(Pos3D));

                        Rect cell = GetCellsRect(col, row, colspan, rowspan);
                        double x = cell.X - from.X;
                        double y = cell.Y - from.Y;
                        double w = cell.Width - from.Width;
                        double h = cell.Height - from.Height;

                        ani.DoOnNext +=
                            q =>
                            SetPlace(element, new Rect(from.X + x*q, from.Y + y*q, from.Width + w*q, from.Height + h*q));
                        ani.DoOnCompleted +=
                            () =>
                            element.SetPlace(Rect.Empty).SetColumn(col).SetColumnSpan(colspan).SetRow(row).SetRowSpan(
                                rowspan);
                        ani.OnNext(i);
                    });
        }

        public Animation MoveToCell(FrameworkElement element, CellsRegion region, TimeSpan howLong = default(TimeSpan),
                                    IEasingFunction easing = null)
        {
            return MoveToCell(element, region.Col, region.Row, region.ColSpan, region.RowSpan, howLong, easing);
        }

        #endregion

        #region SWAPer

        /// <summary>
        /// Меняем элемент местами
        /// </summary>
        /// <param name="child"></param>
        public override void SwapElement(FrameworkElement child)
        {
            base.SwapElement(child);
            int v1 = GetRow(child);
            int v2 = GetColumn(child);
            SetRow(child, v2);
            SetColumn(child, v1);
            v1 = GetRowSpan(child);
            v2 = GetColumnSpan(child);
            SetRowSpan(child, v2);
            SetColumnSpan(child, v1);
        }

        public override void Swap(Boolean withThis = false)
        {
            LineDefinitions v1 = ColumnDefinitions;
            LineDefinitions v2 = RowDefinitions;
            ColumnDefinitions = v2;
            RowDefinitions = v1;
            base.Swap(withThis);
        }

        #endregion

        public double SnapX
        {
            get { return (double) GetValue(SnapXProperty); }
            set { SetValue(SnapXProperty, value); }
        }

        public double SnapY
        {
            get { return (double) GetValue(SnapYProperty); }
            set { SetValue(SnapYProperty, value); }
        }

    

        /// <summary>
        /// Return a rectangle occupied by cells region
        /// </summary>
        /// <param name="column">Column num</param>
        /// <param name="row">Row num</param>
        /// <param name="colSpan">Column Span</param>
        /// <param name="rowSpan">Row Span</param>
        /// <returns></returns>
        public Rect GetCellsRect(int column, int row, int colSpan, int rowSpan)
        {
            return new Rect(ColumnDefinitions.GetCoord(column), RowDefinitions.GetCoord(row),
                            ColumnDefinitions.GetLength(column, colSpan, ActualWidth),
                            RowDefinitions.GetLength(row, rowSpan, ActualHeight));
        }
    }
}