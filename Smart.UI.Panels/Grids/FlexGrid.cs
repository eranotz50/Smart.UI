using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
  /// <summary>
  /// Is much more powerful than Grid because can also use canvas positioning
  /// </summary>
    public class FlexGrid : FlexCanvas
    {
        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// Row definitions
        /// </summary>
        private static readonly DependencyProperty RowDefinitionsProperty = DependencyProperty.Register(
            "RowDefinitions", typeof (LineDefinitions), typeof (FlexGrid), new PropertyMetadata(null, RowsCallback));


        /// <summary>
        /// Columns
        /// </summary>
        private static readonly DependencyProperty ColumnDefinitionsProperty =
            DependencyProperty.Register("ColumnDefinitions", typeof (LineDefinitions), typeof (FlexGrid),
                                        new PropertyMetadata(null, ColumnsCallback));

        public static readonly DependencyProperty RowProperty = DependencyProperty.RegisterAttached("Row", typeof (int),
                                                                                                    typeof (
                                                                                                        FrameworkElement
                                                                                                        ),
                                                                                                    new PropertyMetadata
                                                                                                        (0,
                                                                                                         InvalidateParentMeasureCallback));

        /// <summary>
        /// количество рядков что занимает элемент
        /// </summary>
        public static readonly DependencyProperty RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan",
                                                                                                        typeof (int),
                                                                                                        typeof (
                                                                                                            FrameworkElement
                                                                                                            ),
                                                                                                        new PropertyMetadata
                                                                                                            (1,
                                                                                                             InvalidateParentMeasureCallback));

        /// <summary>
        /// Колонка элемента
        /// </summary>
        public static readonly DependencyProperty ColumnProperty = DependencyProperty.RegisterAttached("Column",
                                                                                                       typeof (int),
                                                                                                       typeof (
                                                                                                           FrameworkElement
                                                                                                           ),
                                                                                                       new PropertyMetadata
                                                                                                           (0,
                                                                                                            InvalidateParentMeasureCallback));

        /// <summary>
        /// количество колонок, что занимает элемент
        /// </summary>
        public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.RegisterAttached(
            "ColumnSpan", typeof (int), typeof (FrameworkElement),
            new PropertyMetadata(1, InvalidateParentMeasureCallback));

       
        public LineDefinitions RowDefinitions
        {
            get { return this.GetOrDefault<LineDefinitions>(RowDefinitionsProperty); }
            set { SetValue(RowDefinitionsProperty, value); }
        }


        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void RowsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as FlexGrid;
            if (p == null) return;
            var old = e.OldValue as LineDefinitions;
            var val = e.NewValue as LineDefinitions;
            p.OnLineDefinitionsChanged(old, val, Orientation.Horizontal);
        }



        public LineDefinitions ColumnDefinitions
        {
            get { return this.GetOrDefault<LineDefinitions>(ColumnDefinitionsProperty); }
            set { SetValue(ColumnDefinitionsProperty, value); }
        }

        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void ColumnsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as FlexGrid;
            if (p == null) return;
            var old = e.OldValue as LineDefinitions;
            var val = e.NewValue as LineDefinitions;
            p.OnLineDefinitionsChanged(old, val, Orientation.Horizontal);
        }

     


        public static int GetRow(FrameworkElement element)
        {
            return (int) element.GetValue(RowProperty);
        }

        public static void SetRow(FrameworkElement element, int value)
        {
            element.SetValue(RowProperty, value);
        }


        public static int GetRowSpan(FrameworkElement element)
        {
            return (int) element.GetValue(RowSpanProperty);
        }

        public static void SetRowSpan(FrameworkElement element, int value)
        {
            element.SetValue(RowSpanProperty, value);
        }


        public static int GetColumn(FrameworkElement element)
        {
            return (int) element.GetValue(ColumnProperty);
        }

        public static void SetColumn(FrameworkElement element, int value)
        {
            element.SetValue(ColumnProperty, value);
        }


        public static int GetColumnSpan(FrameworkElement element)
        {
            return (int) element.GetValue(ColumnSpanProperty);
        }

        public static void SetColumnSpan(FrameworkElement element, int value)
        {
            element.SetValue(ColumnSpanProperty, value);
        }

        /// <summary>
        /// Делает возможность использовать канвасные элементы внутри клетки
        /// </summary>
        public static readonly DependencyProperty RelativeToGridProperty =
            DependencyProperty.RegisterAttached("RelativeToGrid", typeof(Boolean), typeof(FrameworkElement),
                                                new PropertyMetadata(false, InvalidateParentMeasureCallback));


        public static Boolean GetRelativeToGrid(FrameworkElement element)
        {
            return (bool) element.GetValue(RelativeToGridProperty);
        }

        public static void SetRelativeToGrid(FrameworkElement element, Boolean value)
        {
            element.SetValue(RelativeToGridProperty, value);
        }        


        #endregion
#if WPF
        public FlexGrid()
        {

            if(RowDefinitions==null) RowDefinitions = new LineDefinitions();
            if(ColumnDefinitions==null) ColumnDefinitions = new LineDefinitions();

        }
#endif
        /// <summary>
        /// Вынес его инициазилацию в отдельную легко овверайдную функцию
        /// </summary>
        protected override void InitDragManager()
        {
            DragManager = new GridDragManager(this);
        }

        /// <summary>
        /// Used in callbacks
        /// </summary>
        /// <param name="old"></param>
        /// <param name="val"></param>
        /// <param name="orientation">Line </param>
        protected virtual void OnLineDefinitionsChanged(LineDefinitions old, LineDefinitions val,
                                                        Orientation orientation = Orientation.Horizontal)
        {
            if (old != null) old.Update -= InvalidateMeasure;
            if (val == null) return;
            val.Update += InvalidateMeasure;
            val.Orientation = orientation;
            InvalidateMeasure();
        }

        #region SWAPING

        /// <summary>
        /// Swaps ItemLines
        /// </summary>
        protected virtual void SwapLines()
        {
            LineDefinitions cols = ColumnDefinitions;
            LineDefinitions rows = RowDefinitions;
            ColumnDefinitions = rows;
            RowDefinitions = cols;
            InvalidateMeasure();
        }

        #endregion

        #region LAYOUT

        /// <summary>
        /// Овверайдим заполнение элемента с учетом рядков и колонок
        /// </summary>
        /// <param name="child">Элемент на гриде</param>
        /// <param name="constrains">Размеры грида</param>
        /// <returns>Прямоугольник занимаемый расставляемым элементом</returns>
        public override T Populate<T>(FrameworkElement child, Size constrains)
        {
            return GetRelativeToGrid(child)
                       ? RelativePopulate<T>(child, constrains)
                       : AbsolutePopulate<T>(child, constrains);
        }

        public T AbsolutePopulate<T>(FrameworkElement child, Size constrains) where T : Placeholder, new()
        {
            var placer = base.Populate<T>(child, constrains);
            if (ColumnDefinitions.Count > 0)
                if (!placer.HasHorizontalPosition)
                {
                    int column = GetColumn(child);
                    if (column < 0 || column >= ColumnDefinitions.Count) return placer;
                    int right = column + GetColumnSpan(child);
                    placer.Left = ColumnDefinitions.GetCoord(column);
                    if (!ColumnDefinitions[column].IsAuto)
                        placer.Right = ColumnDefinitions.GetRight(right);
                }
            if (RowDefinitions.Count <= 0) return placer;
            if (placer.HasVerticalPosition) return placer;
            int row = GetRow(child);
            if (row < 0 || row >= RowDefinitions.Count) return placer;
            int bottom = row + GetRowSpan(child);
            placer.Top = RowDefinitions.GetCoord(row);
            if (!RowDefinitions[row].IsAuto)
                placer.Bottom = RowDefinitions.GetRight(bottom);
            return placer;
        }

        /// <summary>
        /// Relative positioning, takes into consideration not only columns and rows but canvas params as well
        /// </summary>
        /// <param name="child">Element to be positioned</param>
        /// <param name="constrains"></param>
        public virtual T RelativePopulate<T>(FrameworkElement child, Size constrains) where T : Placeholder, new()
        {
            var size = new Size(double.PositiveInfinity, double.PositiveInfinity);
            var pl = new Placeholder(true);

            if (ColumnDefinitions.Count > 0)
            {
                int column = GetColumn(child);
                int cSpan = GetColumnSpan(child);
                int right = column + cSpan;
                pl.Left = ColumnDefinitions.GetCoord(column);
                pl.Right = double.PositiveInfinity;
                if (column < ColumnDefinitions.Count && !ColumnDefinitions[column].IsAuto)
                    pl.Right = ColumnDefinitions.GetRight(right);
                size.Width = Math.Min(ColumnDefinitions.GetLength(column, cSpan), constrains.Width);
            }

            if (RowDefinitions.Count > 0)
            {
                int row = GetRow(child);
                int rSpan = GetRowSpan(child);
                int bottom = row + rSpan;
                pl.Top = RowDefinitions.GetCoord(row);
                pl.Bottom = double.PositiveInfinity;
                if (row < RowDefinitions.Count && !RowDefinitions[row].IsAuto)
                    pl.Bottom = RowDefinitions.GetRight(bottom);
                size.Height = Math.Min(RowDefinitions.GetLength(row, rSpan), constrains.Height);
            }
            size = constrains.Min(size);
            var placer = base.Populate<T>(child, size);

            if (child.Height.IsMoreThanNull() && child.Height < size.Height)
            {
                if (placer.Top.IsMoreThanNull())
                {
                    if (placer.Bottom.IsValid() == false) placer.Bottom = size.Height - child.Height - placer.Top;
                }
                else if (placer.Bottom.IsValid()) placer.Top = size.Height - child.Height - placer.Bottom;
            }

            if (child.Width.IsMoreThanNull() && child.Width < size.Width)
            {
                if (placer.Left.IsValid())
                {
                    if (placer.Right.IsValid() == false) placer.Right = size.Width - child.Width - placer.Left;
                }
                else if (placer.Right.IsValid()) placer.Left = size.Width - child.Width - placer.Right;
            }
            placer.Add(pl);
            return placer;
        }


        /// <summary>
        /// Measures all children elements and also write their positions to rectangular slots
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Dirty = Dirtiness.Measure;
            Size size = this.ExtractSize(availableSize);
            Space.UpdateSizes(size);
            Size constrains = Space.Canvas.Size();
            UpdateLineDefinitions();
            BeforeMeasurement.OnNext(constrains);
            SmartCollection<FrameworkElement> forMeasure = PrepareManual(constrains);
            constrains = Space.Canvas.Size(); //NEEDED FOR AUTOUPDATES
            foreach (FrameworkElement child in forMeasure) MeasureChild(child, constrains);
            AfterMeasurement.OnNext(constrains);
            return size;
        }


        /// <summary>
        /// Updates lengthes and row/columns params for Row and Columns Definitions, changes panel's size if needed
        /// </summary>
        protected void UpdateLineDefinitions()
        {
            UpdateDeltaForce();

            GrowSelfHorizontal(CountDelta(ColumnDefinitions.DeltaAll, ColumnDefinitions.RealLength, Space.MaxWidth),
                               AlignmentX.Right, ColumnDefinitions.PanelUpdateMode);
            ColumnDefinitions.Length = Space.Canvas.Width;

            GrowSelfVertical(CountDelta(RowDefinitions.DeltaAll, RowDefinitions.RealLength, Space.MaxHeight),
                             AlignmentY.Bottom, RowDefinitions.PanelUpdateMode);
            RowDefinitions.Length = Space.Canvas.Height;
        }


        protected virtual void UpdateDeltaForce()
        {
            var delta = new Point(ColumnDefinitions.DeltaForce, RowDefinitions.DeltaForce);
            if (delta.Equals(default(Point))) return;
            GrowSelf(delta, AlignmentX.Right, AlignmentY.Bottom, PanelUpdateMode.Panel);
        }

        /// <summary>
        /// How much element should grow
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="len"></param>
        /// <param name="whole"></param>
        /// <returns></returns>
        protected double CountDelta(double delta, double len, double whole)
        {
            if (delta.Equals(0)) return 0;
            if (delta > 0)
            {
                double sum = delta + len;
                return sum > whole ? sum - whole : 0;
            }
            return delta;
        }

        /// <summary>
        /// Measures elements that have auto rows and/or columns associated with them and return all other elements to be measured separately
        /// </summary>
        /// <param name="constrains"></param>
        /// <returns></returns>
        protected SmartCollection<FrameworkElement> PrepareManual(Size constrains)
        {
            RowDefinitions.BlockPanelUpdate = ColumnDefinitions.BlockPanelUpdate = true;
            if (RowDefinitions.Autos.Count == 0 && ColumnDefinitions.Autos.Count == 0)
            {
                RowDefinitions.BlockPanelUpdate = ColumnDefinitions.BlockPanelUpdate = false;
                return Children;
            }
            var retval = new SmartCollection<FrameworkElement>();
            foreach (FrameworkElement child in Children)
            {
                if (child.GetPos().HasXY() || !child.GetPlace().IsEmpty)
                {
                    retval.Add(child);
                    continue;
                }
                LineDefinition colDef = ColumnDefinitions.Count > 0 ? ColumnDefinitions[GetColumn(child)] : null;
                var colAuto = colDef != null && colDef.IsAuto;
                if (colAuto)
                {
                    MeasureForAuto(child, constrains);
                    PrepareAuto(constrains, child, colDef);
                }
                LineDefinition rowDef = RowDefinitions.Count > 0 ? RowDefinitions[GetRow(child)] : null;
                var rowAuto = rowDef != null && rowDef.IsAuto;
                if (rowAuto)
                {
                    if (!(colAuto)) MeasureForAuto(child, constrains);
                    PrepareAuto(constrains, child, rowDef, false);
                }
                if (!rowAuto && !colAuto) retval.Add(child);
            }

            UpdateLineDefinitions();
            RowDefinitions.BlockPanelUpdate = ColumnDefinitions.BlockPanelUpdate = false;
            return retval;
        }


        /// <summary>
        /// Setups lineDefsize
        /// </summary>
        /// <param name="constrains"></param>
        /// <param name="child"></param>
        /// <param name="lineDef"></param>
        /// <param name="horizontal">defines whether we work with row or column</param>
        protected void PrepareAuto(Size constrains, FrameworkElement child, LineDefinition lineDef,
                                   bool horizontal = true)
        {
//            var len = Math.Max(lineDef.AbsoluteValue, horizontal?child.DesiredSize.Width:child.DesiredSize.Height);
            double len = horizontal ? child.DesiredSize.Width : child.DesiredSize.Height;
            if (!lineDef.AbsoluteValue.Equals(len) && len.IsValidPositive())
                lineDef.AbsoluteValue = len;
        }

        /// <summary>
        /// Sets only size of the element
        /// needed for auto purposes
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        protected void MeasureForAuto(FrameworkElement child, Size constrains)
        {
            var placer = Populate<CanvasPlaceholder>(child, constrains);
            Size size = placer.GetSize(constrains);
            child.Measure(size);
        }

        #endregion
    }
}