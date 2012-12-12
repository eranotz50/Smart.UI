using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Smart.UI.Controls.Scrollers;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Widgets
{
    public class Wall : UI.Widgets.WidgetGrid, IOrientable, IScrollHolder
    {
        #region DEPENDENCY PROPERTIES

        #region MARGINS

        /// <summary>
        /// distance between columns (if horizontal) or raws (if vertical)
        /// </summary>
        public static readonly DependencyProperty BetweenLinesProperty =
            DependencyProperty.Register("BetweenLines", typeof (RelativeLength), typeof (Wall),
                                        new PropertyMetadata(default(RelativeLength), BetweenLinesCallback));


        /// <summary>
        /// distance between columns (if vertical) or raws (if horizontal)
        /// </summary>
        public static readonly DependencyProperty BetweenOtherLinesProperty =
            DependencyProperty.Register("BetweenOtherLines", typeof (RelativeLength), typeof (Wall),
                                        new PropertyMetadata(default(RelativeLength), BetweenOtherLinesCallback));

        public static readonly DependencyProperty LinesLengthProperty =
            DependencyProperty.Register("LinesLength", typeof (RelativeLength), typeof (Wall),
                                        new PropertyMetadata(default(RelativeLength), LinesCallback));


        public static readonly DependencyProperty OtherLinesLengthProperty =
            DependencyProperty.Register("OtherLinesLength", typeof (RelativeLength), typeof (Wall),
                                        new PropertyMetadata(default(RelativeLength), OtherLinesCallback));

        [TypeConverter(typeof (RelativeLengthConverter))]
        public RelativeLength BetweenLines
        {
            get { return this.GetOrDefault(BetweenLinesProperty, () => new RelativeLength(10)); }
            set { SetValue(BetweenLinesProperty, value); }
        }

        [TypeConverter(typeof (RelativeLengthConverter))]
        public RelativeLength BetweenOtherLines
        {
            get { return this.GetOrDefault(BetweenOtherLinesProperty, () => new RelativeLength(10)); }
            set { SetValue(BetweenOtherLinesProperty, value); }
        }

        [TypeConverter(typeof (RelativeLengthConverter))]
        public RelativeLength LinesLength
        {
            get { return this.GetOrDefault(LinesLengthProperty, () => new RelativeLength(200)); }
            set { SetValue(LinesLengthProperty, value); }
        }

        [TypeConverter(typeof (RelativeLengthConverter))]
        public RelativeLength OtherLinesLength
        {
            get { return this.GetOrDefault(OtherLinesLengthProperty, () => new RelativeLength(200)); }
            set { SetValue(OtherLinesLengthProperty, value); }
        }

        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void BetweenLinesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            var rel = e.NewValue as RelativeLength;
            if (rel == null) return;
            wall.UpdateBetweenLines(rel);
        }

        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void BetweenOtherLinesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            var rel = e.NewValue as RelativeLength;
            if (rel == null) return;
            wall.UpdateBetweenOtherLines(rel);
        }

        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void LinesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            var rel = e.NewValue as RelativeLength;
            if (rel == null) return;
            wall.UpdateLines(rel);
            wall.Dirty = Dirtiness.Measure;
            wall.InvalidateMeasure();
        }


        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void OtherLinesCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            var rel = e.NewValue as RelativeLength;
            if (rel == null) return;
            wall.UpdateOtherLines(rel);
        }

        #endregion

        #region ITEMS REGION

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof (SmartCollection<FrameworkElement>), typeof (Wall),
                                        new PropertyMetadata(default(SmartCollection<FrameworkElement>),
                                                             ItemsChangedCallback));

        public SmartCollection<FrameworkElement> Items
        {
            get { return (SmartCollection<FrameworkElement>) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void ItemsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            wall.OnItemsChanged(e.OldValue as SmartCollection<FrameworkElement>,
                                e.NewValue as SmartCollection<FrameworkElement>);
            wall.InvalidateMeasure();
        }

        #endregion

        #region CELLS AND ORIENTATION

        /// <summary>
        /// How much cells should be placed in a line of elements
        /// </summary>
        public static readonly DependencyProperty CellsInLineProperty =
            DependencyProperty.Register("CellsInLine", typeof (int), typeof (Wall),
                                        new PropertyMetadata(4, CellsInLineCallback));

        /// <summary>
        /// Walls orientation
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof (Orientation), typeof (Wall),
                                        new PropertyMetadata(Orientation.Horizontal, OrientationChangedCallback));

        public int CellsInLine
        {
            get { return (int) GetValue(CellsInLineProperty); }
            set { SetValue(CellsInLineProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void CellsInLineCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            wall.CellsInLineChanged();
            wall.InvalidateMeasure();
        }


        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void OrientationChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wall = d as Wall;
            if (wall == null) return;
            wall.Swap();
        }

        #endregion

        #region ATTACHED

        public static readonly DependencyProperty ContentSizeProperty =
            DependencyProperty.RegisterAttached("ContentSize", typeof (ContentSize), typeof (FrameworkElement),
                                                new PropertyMetadata(ContentSize.Medium, InvalidateMeasureCallback));

        public static void SetContentSize(FrameworkElement element, ContentSize value)
        {
            element.SetValue(ContentSizeProperty, value);
        }

        public static ContentSize GetContentSize(FrameworkElement element)
        {
            return (ContentSize) element.GetValue(ContentSizeProperty);
        }

        #endregion

        #endregion

        public WallCollection2D<FrameworkElement> ItemLines;

        public Wall()
        {
            Init();
        }

        protected virtual void Init()
        {
            Paginator = new Paginator();
            ItemPositionUpdate = new SimpleSubject<Args<FrameworkElement, OrientedCellsRegion, OrientedCellsRegion>>();
            InitLines();
            InitScrolls();
            this.TurnOnAnimations();
        }

        /// <summary>
        /// Turnas animation off
        /// </summary>
        public void TurnOffAnimation()
        {
            ItemPositionUpdate.DoOnNext -= this.ItemPositionChangeHandler;
        }

        /// <summary>
        /// Turns animation on
        /// </summary>
        public void TurnOnAnimations()
        {
            ItemPositionUpdate.DoOnNext += this.ItemPositionChangeHandler;
        }

        /// <summary>
        /// Inits items lines
        /// </summary>
        protected virtual void InitLines()
        {
            ItemLines = new WallCollection2D<FrameworkElement>{NextPageSelector = Paginator.PaginationGenerator(CellsInLine)};
            ItemLines.ItemsAdded.DoOnNext += AddLineHandler;
            ItemLines.ItemsRemoved.DoOnNext += RemoveLineHandler;
            ItemLines.OtherLineTriplets.ItemsAdded.DoOnNext += OtherLinesAddedHandler;
            ItemLines.OtherLineTriplets.ItemsRemoved.DoOnNext += OtherLinesRemovedHandler;
            ItemLines.AnyItemAdded.DoOnNext += MakeDirty;
            ItemLines.AnyItemRemoved.DoOnNext += MakeDirty;
        }

        protected void MakeDirty(FrameworkElement element)
        {
            LinesDirty = true;
        }

        protected override void OnLineDefinitionsChanged(LineDefinitions old, LineDefinitions val,
                                                         Orientation orientation = Orientation.Horizontal)
        {
            if (old == null)
                if (val.PanelUpdateMode == PanelUpdateMode.None) val.PanelUpdateMode = PanelUpdateMode.Canvas;
            base.OnLineDefinitionsChanged(old, val, orientation);
        }

        #region LINE AND ITEM HANDLERS

        /// <summary>
        /// Add line to OtherLineTriplets and adds linedefs to it
        /// </summary>
        /// <param name="line"></param>
        protected void AddLineHandler(WallCollection<FrameworkElement> line)
        {
            Contract.Ensures(LineDefs.Contains(line.Position.Before)
                             && LineDefs.Contains(line.Position.Content)
                             && LineDefs.Contains(line.Position.After));
            Contract.Ensures(line.Position.Content.IsStar && LineDefs.Stars.Contains(line.Position.Content)
                             || line.Position.Content.IsAbsolute && LineDefs.Values.Contains(line.Position.Content));

            if (line.Position == null)
                line.Position = new Triplet<LineDefinition>(new LineDefinition(BetweenLines),
                                                            new LineDefinition(LinesLength),
                                                            new LineDefinition(BetweenLines));
            LineDefs.AddTriplet(line);
        }


        /// <summary>
        /// Handler that remove linedefs of the specified line on its removal from collection
        /// </summary>
        /// <param name="line"></param>
        protected void RemoveLineHandler(WallCollection<FrameworkElement> line)
        {
            LineDefs.RemoveTriplet(line);
        }


        /// <summary>
        /// Fires when a triplet is added to ItemLines.OtherLines
        /// </summary>
        /// <param name="triplet"></param>
        protected void OtherLinesAddedHandler(Triplet<LineDefinition> triplet)
        {
            if (triplet.Before == null) triplet.Before = new LineDefinition(BetweenOtherLines);
            if (triplet.Content == null) triplet.Content = new LineDefinition(OtherLinesLength);
            if (triplet.After == null) triplet.After = new LineDefinition(BetweenOtherLines);
            OtherLineDefs.AddTriplet(triplet);
        }

        /// <summary>
        /// Fires when a triplet is removed from ItemLines.OtherLines
        /// </summary>
        /// <param name="triplet"></param>
        protected void OtherLinesRemovedHandler(Triplet<LineDefinition> triplet)
        {
            OtherLineDefs.RemoveTriplet(triplet);
        }

        #endregion

        #region ITEMS CHANGE

        protected virtual void OnItemsChanged(SmartCollection<FrameworkElement> oldVal,
                                              SmartCollection<FrameworkElement> newVal)
        {
            if (oldVal != null) RemoveOldItems(oldVal);
            if (newVal != null) UpdateItems(newVal);
        }

        protected void RemoveOldItems(SmartCollection<FrameworkElement> old)
        {
            ItemLines.UnFollowFlat(old);
            foreach (FrameworkElement element in old) Children.Remove(element);
            Children.Unfollow(old);
            foreach (FrameworkElement element in old) RemoveChild(element);
            ItemLines.Clear();
            ItemLines.OtherLineTriplets.Clear();
            //this.ItemLines.OtherLineTriplets.ItemsAdded.DoOnNext -= OtherLinesAddedHandler;
            //this.ItemLines.OtherLineTriplets.ItemsRemoved.DoOnNext -= OtherLinesRemovedHandler;
        }


        /// <summary>
        /// Fires when items where changed
        /// </summary>
        protected void UpdateItems(SmartCollection<FrameworkElement> newVal)
        {
            LineDefs.Clear();
            Children.Add(newVal);
            Children.Follow(newVal);
            ItemLines.NextPageSelector = Paginator.PaginationGenerator(CellsInLine);
            ItemLines.FillFollowFlat(newVal);
        }

        #endregion

        #region SCROLLS

        public AbstractScrollBar MainScroller
        {
            get { return Orientation == Orientation.Horizontal ? (AbstractScrollBar) HorizontalScroll : VerticalScroll; }
        }

        public HorizontalScrollBar HorizontalScroll { get; set; }
        public VerticalScrollBar VerticalScroll { get; set; }

        protected virtual void InitScrolls()
        {
            HorizontalScroll = new HorizontalScrollBar();
            VerticalScroll = new VerticalScrollBar();
            Children.Add(HorizontalScroll);
            Children.Add(VerticalScroll);
        }

        #endregion

        #region ORIENTATION RELATED

        public LineDefinitions LineDefs
        {
            get { return Orientation == Orientation.Horizontal ? ColumnDefinitions : RowDefinitions; }
        }

        public LineDefinitions OtherLineDefs
        {
            get { return Orientation == Orientation.Horizontal ? RowDefinitions : ColumnDefinitions; }
        }

        protected override void SwapLines()
        {
            base.SwapLines();
            UpdateLineNums();
            InvalidateMeasure();
        }

        #endregion

        #region MARGIN RELATED

        /// <summary>
        /// Updates margins between cells
        /// </summary>
        /// <param name="rel"></param>
        protected void UpdateBetweenLines(RelativeLength rel)
        {
            foreach (var line in ItemLines.Where(l => l.Position != null))
            {
                line.Position.Before.FromRelativeLength(rel);
                line.Position.After.FromRelativeLength(rel);
            }
            InvalidateMeasure();
        }

        protected void UpdateBetweenOtherLines(RelativeLength rel)
        {
            foreach (var line in ItemLines.OtherLineTriplets.Where(l => l.Before != null && l.After != null))
            {
                line.Before.FromRelativeLength(rel);
                line.After.FromRelativeLength(rel);
            }
            InvalidateMeasure();
        }

        /// <summary>
        /// Updates margins between cells
        /// </summary>
        /// <param name="rel"></param>
        public void UpdateLines(RelativeLength rel)
        {
            foreach (var line in ItemLines.Where(l => l.Position != null))
                line.Position.Content.FromRelativeLength(rel);
            InvalidateMeasure();
        }

        /// <summary>
        /// Updates margins between cells
        /// </summary>
        /// <param name="rel"></param>
        public void UpdateOtherLines(RelativeLength rel)
        {
            foreach (var line in ItemLines.OtherLineTriplets.Where(l => l.Content != null))
                line.Content.FromRelativeLength(rel);
            InvalidateMeasure();
        }

        #endregion

        #region ITEMS AND CELLS RELATED

        public SimpleSubject<Args<FrameworkElement, OrientedCellsRegion, OrientedCellsRegion>> ItemPositionUpdate;
        public IPaginator Paginator;
        private bool _linesDirty;
    

        public bool LinesDirty
        {
            get { return _linesDirty; }
            set
            {
                if (_linesDirty == value) return;
                _linesDirty = value;
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// fires when quantity of cells in a line has been changed
        /// </summary>
        protected void CellsInLineChanged()
        {
            ItemLines.NextPageSelector = Paginator.PaginationGenerator(CellsInLine);
            if (ItemLines.Count > 0) ItemLines.ReArrange();
        }


        /// <summary>
        /// updates row and col position for each element
        /// </summary>
        public void UpdateLineNums()
        {
            int maxCells =
                ItemLines.Max(
                    l =>
                    l.Aggregate(0, (current, element) => SetLineNumsForItem(element, l.Position.Content.Num, current)));
            while (ItemLines.OtherLineTriplets.Count > maxCells) ItemLines.OtherLineTriplets.Pop();

            LinesDirty = false;
        }

        /// <summary>
        /// Sets row and column nums for an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="lNum"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        protected int SetLineNumsForItem(FrameworkElement item, int lNum, int current)
        {
            int cells = Paginator.CellsIn(item);
            int count = current + cells;
            ItemLines.UpdateOtherLineDefs(count);
            int otherSpan = ItemLines.GetLineSpan(current, cells);

            //Setting line and other line definitions
            OrientedCellsRegion oldR = GetOrientedRegion(item);
            var newR = new OrientedCellsRegion(lNum, ItemLines.OtherLineTriplets[current].Content.Num, 1, otherSpan);
            if (!oldR.Equals(newR))
            {
                item.SetOrientedCellsRegion(newR, Orientation);
                if (!GetSlot(item).IsEmpty)
                    ItemPositionUpdate.OnNext(new Args<FrameworkElement, OrientedCellsRegion, OrientedCellsRegion>(
                                                  item, oldR, newR));
            }

            return count;
        }

        /// <summary>
        /// When item changes its position
        /// </summary>
        /// <param name="args"></param>
        protected void ItemPositionChangeHandler(Args<FrameworkElement, OrientedCellsRegion, OrientedCellsRegion> args)
        {
            if (this.MovementTime == default(TimeSpan)) return;            
            FrameworkElement item = args.Param0;
            CellsRegion region = args.Param2.ToCellsRegion(Orientation);
            InvisibleChange(item, region);            
            SetMovement(item, MoveInCellsTo(item, region, MovementTime, Easings.CubicEaseInOut).Go());
        }

        /// <summary>
        /// Gets oriented region
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public OrientedCellsRegion GetOrientedRegion(FrameworkElement child)
        {
            return OrientedCellsRegion.FromRegion(GetCellsRegion(child), Orientation);
        }


        /// <summary>
        /// Count total number of cells
        /// </summary>
        /// <returns></returns>
        public int CountCells()
        {
            return ItemLines.SelectMany(line => line).Sum(item => Paginator.CellsIn(item));
        }

        #endregion

      

        #region MEASURE OVERRIDE RELATED

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!LinesDirty) return base.MeasureOverride(availableSize);
            RowDefinitions.UpdateNums();
            ColumnDefinitions.UpdateNums();
            UpdateLineNums(); //NAPILNIK            
            return base.MeasureOverride(availableSize);
        }

        #endregion
    }
}