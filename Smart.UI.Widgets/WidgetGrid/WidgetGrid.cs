using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Panels;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Widgets
{
    public class WidgetGrid : SmartGrid
    {
        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// Time for element's parking animation in widgetgrid
        /// </summary>
        public static readonly DependencyProperty ParkingDurationProperty =
            DependencyProperty.Register("ParkingDuration", typeof (TimeSpan), typeof (WidgetGrid),
                                        new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 400)));

        public static readonly DependencyProperty ColsNumProperty =
            DependencyProperty.Register("ColsNum", typeof (int), typeof (WidgetGrid),
                                        new PropertyMetadata(0, InvalidateMeasureCallback));

        public static readonly DependencyProperty RowsNumProperty =
            DependencyProperty.Register("RowsNum", typeof (int), typeof (WidgetGrid),
                                        new PropertyMetadata(0, InvalidateMeasureCallback));


        /// <summary>
        /// If a drag and droped object has row or colspan, than keep it unchanged on parking
        /// </summary>
        public static readonly DependencyProperty PreserveSpansProperty =
            DependencyProperty.Register("PreserveSpans", typeof (Boolean), typeof (WidgetGrid),
                                        new PropertyMetadata(false));
        /*
        /// <summary>
        /// Defines duration for parking animation
        /// </summary>
        public TimeSpan ParkingDuration
        {
            get { return (TimeSpan) GetValue(ParkingDurationProperty); }
            set { SetValue(ParkingDurationProperty, value); }
        }
        */

        private TimeSpan _movementTime = new TimeSpan(0, 0, 0, 0, 400);
        /// <summary>
        /// Time for and item to move in cell
        /// </summary>
        public TimeSpan MovementTime
        {
            get { return _movementTime; }
            set { _movementTime = value; }
        }


        /// <summary>
        /// setter and getter to easy set up cols in xaml (it automaticaly adds new LineDefinitions to ColumnDefinitions untul total count reaches colsnum value)
        /// </summary>
        public int ColsNum
        {
            get
            {
                //return ColumnDefinitions.Count;
                SetValue(ColsNumProperty, ColumnDefinitions.Count);
                return (int)GetValue(ColsNumProperty);
            }
            set
            {
                if (value <= 0 || value.Equals(ColsNum)) return;
                ColumnDefinitions.MakeNum(value);
                SetValue(ColsNumProperty, ColumnDefinitions.Count);
            }
        }

        /// <summary>
        /// setter and getter to easy set up cols in xaml (it automaticaly adds new LineDefinitions to RowDefinitions untul total count reaches rowsnum value)
        /// </summary>        
        public int RowsNum
        {
            get
            {
                SetValue(RowsNumProperty, RowDefinitions.Count);
                return (int) GetValue(RowsNumProperty);
            }
            set
            {
                if (value <= 0 || value.Equals(RowsNum)) return;
                RowDefinitions.MakeNum(value);
                SetValue(RowsNumProperty, RowDefinitions.Count);
            }
        }
        
        public Boolean PreserveSpans
        {
            get { return (Boolean) GetValue(PreserveSpansProperty); }
            set { SetValue(PreserveSpansProperty, value); }
        }

        #endregion

        #region ATTACHED DEPENDENCY PROPERTIES

        public static TimeSpan DefaultMovementTime = new TimeSpan(0, 0, 0, 0, 500);

        /// <summary>
        /// Every element has its movement. 
        /// </summary>
        public static readonly DependencyProperty MovementProperty =
            DependencyProperty.RegisterAttached("Movement", typeof (Animation), typeof (FrameworkElement),
                                                new PropertyMetadata(default(Animation), MovementCallback));

        public static void SetMovement(FrameworkElement element, Animation value)
        {
            element.SetValue(MovementProperty, value);
        }

        public static Animation GetMovement(FrameworkElement element)
        {
            return element.GetOrDefault(MovementProperty, () => new Animation(DefaultMovementTime));
        }


        /// <summary>
        /// Redraws wall when somethink dramatically changes
        /// </summary>
        public static void MovementCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldV = e.OldValue as Animation;
            if (oldV != null) oldV.Pause();
            //oldV.OnCompleted();
            //var newV = e.NewValue as Animation;
        }

        #endregion

       

        /// <summary>
        /// Checks whether projection of the element onto widgetgrid intersects any of its nonadorner and nondecorating childrens
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected Boolean CheckProjection(FrameworkElement element)
        {
            Rect bounds = element.GetRelativeRect(this);
            CellsRegion cells = MeasureCellsRegion(bounds);
            return
                this.ChildrenInCells<FrameworkElement>(cells).Any(
                    e => e.GetElementType() == ElementType.Normal && e != element);
        }

        /// <summary>
        /// Default handler that decides if the function should park the object or not
        /// </summary>
        /// <param name="fly"></param>
        /// <returns></returns>
        protected override bool AllowDockHandler(ObjectFly fly)
        {
            return fly.DockMode != DockMode.DockOnFreeSpace || !CheckProjection(fly.Target);
            //return base.AllowDock(fly);
        }

        #region CELLS REGIONS AND RECTANGLES

        #region MEASURE CELL REGION

        public CellsRegion MeasureCellsRegion(FrameworkElement target)
        {
            return MeasureCellsRegion(target, target.GetRelativeRect(this));
        }

        public CellsRegion MeasureCellsRegion(FrameworkElement target, Rect bounds)
        {
            return PreserveSpans
                       ? MeasureCellsRegion(bounds, target.GetColumnSpan(), target.GetRowSpan())
                       : MeasureCellsRegion(bounds);
        }

        #endregion

        public CellsRegion GetCellsRegion(FrameworkElement target)
        {
            return new CellsRegion(target.GetColumn(), target.GetRow(), target.GetColumnSpan(), target.GetRowSpan());
        }


        /// <summary>
        /// Returns a retangle occupied by a cell region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public Rect GetCellsRect(CellsRegion region)
        {
            return GetCellsRect(region.Col, region.Row, region.ColSpan, region.RowSpan);
        }


        /// <summary>
        /// Returns a retangle occupied by a cell region extracted from child
        /// </summary>
        /// <param name="child">Child element from which cells rect will be extracted</param>
        /// <returns></returns>
        public Rect GetCellsRect(FrameworkElement child)
        {
            return GetCellsRect(GetColumn(child), GetRow(child), GetColumnSpan(child), GetRowSpan(child));
        }


        public CellsRegion MeasureCellsRegion(Rect bounds, int colSpan, int rowSpan)
        {
            bounds = bounds.ShiftToPlace(new Rect(0, 0, ActualWidth, ActualHeight));
            Tuple<LineDistance, LineDistance> distLeft = GetColumnRowDistance(bounds.TopLeft());
            return
                new CellsRegion(distLeft.Item1.Num, distLeft.Item2.Num, colSpan, rowSpan).PlaceInsideGrid(
                    ColumnDefinitions.Count, RowDefinitions.Count);
        }

        /// <summary>
        /// Gets cells region (row/cols+spans)
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public CellsRegion MeasureCellsRegion(Rect bounds)
        {
            bounds = bounds.ShiftToPlace(new Rect(0, 0, ActualWidth, ActualHeight));
            Tuple<LineDistance, LineDistance> distLeft = GetColumnRowDistance(bounds.TopLeft());
            bounds.X += distLeft.Item1.Dist;
            bounds.Y += distLeft.Item2.Dist;
            var cells = new CellsRegion(distLeft.Item1.Num, distLeft.Item2.Num);

            Tuple<LineDistance, LineDistance> distRight = GetColumnRowDistance(bounds.BottomRight());
            cells.ColSpan = distRight.Item1.Num - cells.Col;
            cells.RowSpan = distRight.Item2.Num - cells.Row;
            if (cells.ColSpan <= 0) cells.ColSpan = 1;
            if (cells.RowSpan <= 0) cells.RowSpan = 1;
            return cells;
        }

        


        /// <summary>
        /// Fits element into rectangle using cells and relativeCanvas left and right
        /// </summary>
        /// <param name="element"> </param>
        /// <param name="rect"></param>     
        public void PlaceInCells(FrameworkElement element, Rect rect)
        {
            PlaceInCells(element, rect, GetCellsRegion(element));
        }

        /// <summary>
        /// Fits element into rectangle using cells and relativeCanvas left and right
        /// </summary>
        /// <param name="element"> </param>
        /// <param name="place"> </param>
        /// <param name="region"> </param>     
        public void PlaceInCells(FrameworkElement element, Rect place, CellsRegion region)
        {
            Contract.Requires(place!=Rect.Empty);
           // Contract.Ensures(!GetRelativeToGrid(element) && this.GetCellsRegion(ele));
            Rect rect = this.GetCellsRect(region);
            if (place.RoundEquals(rect))            
                element.MakeAbsolute();
            else
                element.MakeRelative(place.X - rect.X, place.Top - rect.Top, rect.Right - place.Right, rect.Bottom - place.Bottom);    
        }


        /// <summary>
        /// Changes cells regions silently (slot stays the same but cols and rows move)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="newR"></param>
        public Rect InvisibleChange(FrameworkElement element, CellsRegion newR)
        {
            return InvisibleChange(element, GetCellsRegion(element), newR);
        }

        /// <summary>
        /// Changes cells regions silently (slot stays the same but col and rowspance move
        /// </summary>
        /// <param name="element"></param>
        /// <param name="oldR">Old cellregion </param>
        /// <param name="newR">New cellregion</param>
        public Rect InvisibleChange(FrameworkElement element, CellsRegion oldR, CellsRegion newR)
        {
            return this.InvisibleChange(element, element.GetSlot(), oldR, newR);
        }

        /// <summary>
        /// Changes cells regions silently (slot stays the same but col and rowspance move)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="slot">Position of the element </param>
        /// <param name="oldR">Old cellregion </param>
        /// <param name="newR">New cellregion</param>
        public Rect InvisibleChange(FrameworkElement element, Rect slot, CellsRegion oldR, CellsRegion newR)
        {
            if (!oldR.Equals(newR)) newR.SetPosition(element);
            PlaceInCells(element, slot, newR);
            return slot;
        }

        #endregion

        #region CONTENT ITEM WORKING WITH CONTENTITEMS

            #region OVERRIDES
        
                public override bool Shift(FrameworkElement child, Point shift)
                {
                    return IsContentItem(child)? ShiftItem(child,shift): base.Shift(child, shift);
                }                      
               

                public override void ParkChild(FrameworkElement child)
                {
                    if (MovementTime != default(TimeSpan))
                        this.MoveInCellsTo(child, this.GetCellsRegion(child), MovementTime).Go();
                    else                  
                        child.MakeAbsolute();
                }

              
        
            #endregion

        protected virtual Boolean ShiftItem(FrameworkElement child, Point shift)
        {
            child.MakeRelative();
            ///SetRelativeToGrid(child,true);
            Extractor.Shift(child, shift); return true;
        }


        public override void ReceiveChild(FrameworkElement child, Rect oldPlace, Rect newPlace)
        {
            if (IsContentItem(child)) ReceiveItem(child, oldPlace, newPlace); else base.ReceiveChild(child, oldPlace, newPlace);
        }

        protected virtual Boolean IsContentItem(FrameworkElement child)
        {
            return GetElementType(child) == ElementType.Normal;
        }
        

        
        protected virtual void ReceiveItem(FrameworkElement element, Rect oldPlace, Rect newPlace)
        {      
            Contract.Requires(!newPlace.Equals(Rect.Empty));
            Contract.Ensures(this.GetCellsRegion(element).Equals(MeasureCellsRegion(newPlace)));

            this.ClearPosition(element);
            var oldRegion = this.GetCellsRegion(element);
            var measurement = MeasureCellsRegion(newPlace);
            InvisibleChange(element, newPlace,oldRegion, measurement);
            if (PreserveSpans) element.SetRowSpan(oldRegion.RowSpan).SetColumnSpan(oldRegion.ColSpan);
            
        }

        #endregion

        #region PARKING

       
        public void ParkToCanvas(FrameworkElement cell)
        {
            Rect slot = GetPlace(cell); //GetSlot(cell);
            if (slot.Equals(Rect.Empty)) return;
            Extractor.SetCoords(slot, new Size(ActualWidth, ActualHeight), cell);
        }
        /*
      
        /// <summary>
        /// Animationly parking an element to the nearest grid cell
        /// </summary>
        /// <param name="element"></param>
        /// <param name="newPlace"> </param>
        /// <returns></returns>
        public Animation GridParking(FrameworkElement element, Rect newPlace)
        {
            element.SetPlace(newPlace);
            CellsRegion region = MeasureCellsRegion(element, newPlace);
            return MoveToCell(element, region, ParkingDuration, Easings.CubicEaseInOut);
        }


        /// <summary>
        /// Animationly parking an element to the nearest grid cell
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public Animation GridParking(FrameworkElement element)
        {
            Rect bounds = element.GetBounds();
            return GridParking(element, bounds);
        }
        */
        #endregion

        #region ZOOMING

        public Animation Zoom(double z, TimeSpan howLong, IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        Size size = Space.Canvas.Size();
                        Point shift = Space.Panel.TopLeft();
                        var delta = new Point(size.Width*(z - 1), size.Height*(z - 1));
                        var posDelta = new Point(delta.X/2, delta.Y/2);
                        ani.DoOnNext += q =>
                                            {
                                                Space.CanvasChange.FireChange(
                                                    Space.Canvas.SetSize(size.Width + delta.X*q,
                                                                         size.Height +
                                                                         delta.Y*q));
                                                Space.PanelChange.FireChange(new Rect(shift.X + posDelta.X*q,
                                                                                      shift.Y + posDelta.Y*q,
                                                                                      Space.Panel.Width,
                                                                                      Space.Panel.Height));
                                            };
                        ani.OnNext(i);
                        ani.DoOnCompleted = () =>
                                                {
                                                    Space.CanvasChange.FireChange(
                                                        Space.Canvas.SetSize(size.Width + delta.X,
                                                                             size.Height + delta.Y));
                                                    Space.PanelChange.FireChange(
                                                        Space.Panel.SetCoords(shift.X + posDelta.X,
                                                                              shift.Y + posDelta.Y));
                                                };
                    });
        }

        public void ZoomAt(FrameworkElement element)
        {
            Space.ZoomAt(element.GetRelativeRect(this));
        }


        public void ZoomAt(Point zoom, FrameworkElement element)
        {
            Space.ZoomAt(zoom, element.GetRelativeRect(this));
        }

       

        #endregion
    }
}