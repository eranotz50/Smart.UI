using System;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Smart.UI.Panels;
using Smart.Classes.Subjects;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Structs;

using Smart.Classes.Extensions;

namespace Smart.UI.Controls.Scrollers
{
    [TemplatePart(Name = "Root", Type = typeof (SmartGrid))]
    [TemplatePart(Name = "Substract", Type = typeof (Rectangle))]
    [TemplatePart(Name = "Slider", Type = typeof (Rectangle))]
    [TemplatePart(Name = "Shadow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "BackwardArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "BackwardFastArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardFastArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardSplitter", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "BackwardSplitter", Type = typeof (FrameworkElement))]
    public abstract class AbstractScrollBar : SmartControl, IOrientable
    {
        private const int DefaultZindex = 1000;


        public static readonly DependencyProperty SensivityProperty =
            DependencyProperty.Register("Sensivity", typeof (double), typeof (AbstractScrollBar),
                                        new PropertyMetadata(0.02));

        /// <summary>
        /// Canvas from panel in order to manipulate it's canvas and panel rects
        /// </summary>
        public CanvasPlace Space;

        public Action<Len, Len> UpdatePosition;

        protected AbstractScrollBar()
        {
            if (UpdatePosition == null) UpdatePosition = DefaultUpdatePositionHandler;
        }

        public double Sensivity
        {
            get { return (double) GetValue(SensivityProperty); }
            set { SetValue(SensivityProperty, value); }
        }

        #region  SCROLL POSITION AND MOVEMENTS, I extracted it to separate variables with change handlers to make binding easier

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Len), typeof(AbstractScrollBar),
                                        new PropertyMetadata(new Len(), ChangePositionCallback));

        public SimpleSubject<Tuple<Len, Len>> PositionChanged;

        public Len Position
        {
            get { return (Len) GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Scollposition change handler
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void ChangePositionCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scroller = d as AbstractScrollBar;
            if (scroller == null) return;
            var oldVal = (Len) e.OldValue;
            var newVal = (Len) e.NewValue;
            if (oldVal.IsNear(newVal)) return;
            scroller.PositionChanged.OnNext(new Tuple<Len, Len>(oldVal, newVal));
            scroller.InvalidateMeasure();
        }

        #endregion

        #region ELEMENTS OF TEMPLATE

        public FrameworkElement BackwardArrow;
        public FrameworkElement BackwardFastArrow;
        public FrameworkElement BackwardSplitter;
        public FrameworkElement ForwardArrow;
        public FrameworkElement ForwardFastArrow;
        public FrameworkElement ForwardSplitter;
        public FlexCanvas Root;
        public Rectangle Shadow;
        public Rectangle Slider;
        public Rectangle Substract;

        #endregion

        #region ORIENTATION RELATED

        private Orientation _orientation = Orientation.Vertical;

        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        #endregion

        public virtual void DefaultUpdatePositionHandler(Len oldVal, Len newVal)
        {
            Position = BoundedLen(oldVal, newVal);
        }

        protected Len BoundedLen(Len oldVal, Len newVal)
        {
            if (newVal.End > newVal.Whole)
            {
                if (newVal.Length > oldVal.Length)
                    newVal.Length = newVal.Whole - newVal.Start;
                else if (newVal.Start > oldVal.Start) newVal.Start = newVal.Whole - newVal.Length;
                return newVal;
            }
            if (newVal.Start < 0) newVal.Start = 0;
            return newVal;
        }


        /// <summary>
        /// Fires when is added to stage
        /// </summary>
        /// <param name="panel"></param>
        protected override void AddedToStageHandler(SimplePanel panel)
        {
            this.AfterTemplateApplied = () =>
                                            {
                                                panel.BeforeArrange.DoOnNext += BeforeArrangeHandler;
                                                panel.AfterArrange.DoOnNext += AfterArrangeHandler;
                                                Space = panel.Space;
                                            };

        }

        protected virtual void AfterArrangeHandler(Size size)
        {
            UpdatePosition(Position, this.Len(Space.RelativeRect));
        }


        protected virtual void BeforeArrangeHandler(Size size)
        {
            double len = this.Length(Space.Panel);
            double rol = this.OtherLength(Root);
            double olen = this.OtherLength(Space.Panel);

            //this.SetLength(Root, Space.Panel);
            double otherCoord = this.OtherCoord(Space.Panel) + olen - rol;

            Rect slot = this.MakeRect(this.Coord(Space.Panel), otherCoord, len, rol);
            this.SetSlot(slot);
            Visibility = this.Length(Space.RelativeRect).IsNear(1.0) ? Visibility.Collapsed : Visibility.Visible;
        }

        protected override void RemovedFromStageHandler(SimplePanel panel)
        {
            panel.AfterArrange.DoOnNext -= AfterArrangeHandler;
            panel.BeforeArrange.DoOnNext -= BeforeArrangeHandler;
            Space = null;
        }


        /// <summary>
        /// Fires when scroller position has been changed
        /// </summary>
        /// <param name="change"></param>
        protected virtual void OnPositionChange(Tuple<Len, Len> change)
        {
            UpdateCanvas(change.Item2);
            SelfPos();
            PosBySpace(Slider);
            PosBySpace(Shadow);
        }

        #region SCROLLER AND SLIDER POSITIONING

        /// <summary>
        /// Positions scroller and all of its elements
        /// </summary>
        protected void SelfPos()
        {
            this.SetLength(Root, Space.Panel);
            this.SetCoord(Space.Panel)
                .SetLength(Space.Panel)
                .SetOtherCoord(this.OtherCoord(Space.Panel) + this.OtherLength(Space.Panel) - this.OtherLength(Root))
                .SetOtherLength(Root);
        }


        /// <summary>
        /// Updates panel canvas if needed
        /// </summary>
        /// <param name="len"></param>
        protected void UpdateCanvas(Len len)
        {
            if (this.HasLength(Space.RelativeRect, len)) return;
            Space.RelativeRect = this.WithLength(Space.RelativeRect, len);
        }

        /// <summary>
        /// Poses slider
        /// </summary>
        public void PosBySpace(FrameworkElement element)
        {
            Rect rect = Space.RelativeRect;
            double scrollBarCoord = this.Coord(rect)*this.Length(Space.Panel);
            double scrollBarLength = this.Length(Space.Panel)*this.Length(rect);
            this.SetCoord(element, scrollBarCoord).SetLength(element, scrollBarLength);
        }

        #endregion

        #region PARTS HANDLERS

        /// <summary>
        /// Add handlers to the panel and templates parts
        /// </summary>
        protected virtual void InitHandlers()
        {
            ScrollDragSubject.DoOnNext += OnScrollerMove;
            SplitBackwardDragSubject.DoOnNext += BackwardSplitHandler;
            SplitForwardDragSubject.DoOnNext += ForwardSplitHandler;
            ForwardClick.DoOnNext += ForwardHandler;
            BackwardClick.DoOnNext += BackwardHandler;
        }

        protected virtual void ForwardHandler(EventPattern<MouseButtonEventArgs> obj)
        {
            Position = new Len(Position.Start + Sensivity, Position.Length);
        }

        protected virtual void BackwardHandler(EventPattern<MouseButtonEventArgs> obj)
        {
            Position = new Len(Position.Start - Sensivity, Position.Length);
        }


        protected virtual void OnScrollerMove(ObjectFly fly)
        {
            Len pos = Position;
            double shift = this.Coord(Space.ToRelativePoint(fly.ShiftPos));
            Position = new Len(pos.Start + shift, pos.Length);
        }

        protected virtual void ForwardSplitHandler(ObjectFly fly)
        {
            Len pos = Position;
            double shift = this.Coord(Space.ToRelativePoint(fly.ShiftPos));
            Position = new Len(pos.Start - shift, pos.Length + shift);
        }

        protected virtual void BackwardSplitHandler(ObjectFly fly)
        {
            Len pos = Position;
            double shift = this.Coord(Space.ToRelativePoint(fly.ShiftPos));
            Position = new Len(pos.Start + shift, pos.Length - shift);
        }

        #endregion

        #region SUBJECTS

        public SimpleSubject<EventPattern<MouseButtonEventArgs>> BackwardClick;
        public SimpleSubject<EventPattern<MouseButtonEventArgs>> ForwardClick;
        public SimpleSubject<ObjectFly> ScrollDragSubject;
        public SimpleSubject<ObjectFly> SplitBackwardDragSubject;
        public SimpleSubject<ObjectFly> SplitForwardDragSubject;

        /// <summary>
        /// Assigns subjects for events and add handlers
        /// </summary>
        protected virtual void InitSubjects()
        {
            PositionChanged = new SimpleSubject<Tuple<Len, Len>>(OnPositionChange);
            //this.TargetPositionChanged = new SimpleSubject<Tuple<Len, Len>>(this.OnTargetPositionChange);
            ForwardClick = new SimpleSubject<EventPattern<MouseButtonEventArgs>>();
            ForwardClick.Add(ForwardArrow.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonDown"));
            ForwardClick.Add(ForwardFastArrow.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonDown"));

            BackwardClick = new SimpleSubject<EventPattern<MouseButtonEventArgs>>();
            BackwardClick.Add(BackwardArrow.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonDown"));
            BackwardClick.Add(BackwardFastArrow.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonDown"));


            ScrollDragSubject = new SimpleSubject<ObjectFly>();
            Slider.SetDragSubject(ScrollDragSubject);
            SplitForwardDragSubject = new SimpleSubject<ObjectFly>();
            //this.ForwardSplitter.SetDragSubject(this.SplitForwardDragSubject);
            SplitBackwardDragSubject = new SimpleSubject<ObjectFly>();
            //this.BackwardSplitter.SetDragSubject(this.SplitBackwardDragSubject);                     
        }

        #endregion

        #region TEMPLATE RELATED

        public Action AfterTemplateApplied;
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            InitTemplateParts();
            InitSubjects();
            InitHandlers();

            Width = Root.Width;
            Height = Root.Height;
            Opacity = 0.5;
            MouseEnter += (s, e) => this.AnimateProperty(OpacityProperty, 1.0, new TimeSpan(0, 0, 0, 0, 100)).Go();
            MouseLeave += (s, e) => this.AnimateProperty(OpacityProperty, 0.5, new TimeSpan(0, 0, 0, 0, 100)).Go();
            if (this.AfterTemplateApplied != null) this.AfterTemplateApplied();

        }

        /// <summary>
        /// Assigns templates parts to corresponding properties
        /// </summary>
        protected virtual void InitTemplateParts()
        {
            if (Template == null) return;
            Root = FromTemplate<FlexCanvas>("Root");
            Root.DragEnabled = true;
            Root.WaitBeforeDrag = default(TimeSpan);
            Root.SetDragMode(DragMode.None);
            Root.Space.UpdateMode = CanvasUpdateMode.KeepCanvas;
            Slider = FromTemplate<Rectangle>("Slider").SetDragCanvas("NearestParent").SetDragMode(DragMode.Custom);
            Shadow =
                FromTemplate<Rectangle>("Shadow").SetDragMode(DragMode.None).SetDragCanvas("NearestParent").SetZIndex(
                    -100);
            Substract = FromTemplate<Rectangle>("Substract");
            Substract.SetDragMode(DragMode.None);

            BackwardSplitter =
                FromTemplate<FrameworkElement>("BackwardSplitter").SetDragCanvas("NearestParent").SetDragMode(
                    DragMode.Custom).SetElementType(ElementType.DecorationAdorner);
            ForwardSplitter =
                FromTemplate<FrameworkElement>("ForwardSplitter").SetDragCanvas("NearestParent").SetDragMode(
                    DragMode.Custom).SetElementType(ElementType.DecorationAdorner);
            BackwardArrow = FromTemplate<FrameworkElement>("BackwardArrow").SetCursor(Cursors.Hand);
            ForwardArrow = FromTemplate<FrameworkElement>("ForwardArrow").SetCursor(Cursors.Hand);
            BackwardFastArrow = FromTemplate<FrameworkElement>("BackwardFastArrow").SetCursor(Cursors.Hand);
            ForwardFastArrow = FromTemplate<FrameworkElement>("ForwardFastArrow").SetCursor(Cursors.Hand);
            this.SetZIndex(DefaultZindex);
        }

        #endregion
    }
}