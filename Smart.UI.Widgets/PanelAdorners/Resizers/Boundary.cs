using System;
using System.Windows;
using System.Windows.Input;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class Boundary<TSide, TCorner> where TSide : FrameworkElement, new() where TCorner : FrameworkElement, new()
    {
        public static int DefZindex = 1000000;
        public FlexCanvas Host;
        private double _linesOpacity = 0.2;

        /// <summary>
        /// Target of the boundare, an element around wich all boundary parts are situated
        /// </summary>
        private FrameworkElement _target;

        public Boundary()
        {
            // this.ActivationSubject = new SimpleSubject<FrameworkElement>();
            InitSides();
            InitCorners();
        }

        #region INITS

        public FrameworkElement GetTarget()
        {
            return _target;
        }

        protected virtual void InitSides()
        {
            Sides = new SmartCollection<TSide>
                        {
                            (Left = new TSide()),
                            (Right = new TSide()),
                            (Top = new TSide()),
                            (Bottom = new TSide())
                        };

            SetDefParams(Left).SetCursor(Cursors.SizeWE).SetDragSubject(new GrowLeftSubject {TargetGetter = GetTarget});
            SetDefParams(Right).SetCursor(Cursors.SizeWE).SetDragSubject(new GrowRightSubject {TargetGetter = GetTarget});
            ;
            SetDefParams(Bottom).SetCursor(Cursors.SizeNS).SetDragSubject(new GrowBottomSubject
                                                                              {TargetGetter = GetTarget});
            ;
            SetDefParams(Top).SetCursor(Cursors.SizeNS).SetDragSubject(new GrowTopSubject {TargetGetter = GetTarget});
            ;
        }

        protected virtual void InitCorners()
        {
            Corners = new SmartCollection<TCorner>
                          {
                              (TopLeft = new TCorner()),
                              (TopRight = new TCorner()),
                              (BottomLeft = new TCorner()),
                              (BottomRight = new TCorner())
                          };

            SetDefParams(TopLeft).SetCursor(Cursors.SizeNWSE).SetDragSubject(new GrowTopLeftSubject
                                                                                 {TargetGetter = GetTarget});
            ;
            SetDefParams(TopRight).SetCursor(Cursors.SizeNESW).SetDragSubject(new GrowTopRightSubject
                                                                                  {TargetGetter = GetTarget});
            ;
            SetDefParams(BottomLeft).SetCursor(Cursors.SizeNESW).SetDragSubject(new GrowBottomLeftSubject
                                                                                    {TargetGetter = GetTarget});
            ;
            SetDefParams(BottomRight).SetCursor(Cursors.SizeNWSE).SetDragSubject(new GrowBottomRightSubject
                                                                                     {TargetGetter = GetTarget});
            ;
        }

        protected FrameworkElement SetDefParams(FrameworkElement source)
        {
            return
                source
                    .SetZIndex(DefZindex)
                    .SetDragMode(DragMode.Custom)
                    .SetDragCanvas("NearestParent")
                    .SetElementType(Target == Host ? ElementType.DecorationAdorner : ElementType.Adorner);
        }

        #endregion

        public FrameworkElement Target
        {
            get { return _target ?? Host; }
            set
            {
                if (value == _target) return;
                if (Host == null && value != null) Host = value.GetNearestDragParent<FlexCanvas>();
                if (_target != null) UnSubscribe(_target);
                _target = value; //??Host;
                if (value != null) Subscribe(value);
            }
        }

        /// <summary>
        /// Прозрачность линии ресайза
        /// </summary>
        public double LinesOpacity
        {
            get { return _linesOpacity; }
            set
            {
                if (value.Equals(LinesOpacity)) return;
                _linesOpacity = value;
                PaintSides();
            }
        }

        public Boolean Activated
        {
            get { return Target != null; }
        }

        #region SIDES

        public TSide Bottom;
        public TSide Left;
        public TSide Right;

        /// <summary>
        /// Стороны
        /// </summary>
        public SmartCollection<TSide> Sides;

        public TSide Top;


        private Thickness _linesThickness = new Thickness(5, 5, 5, 5);

        /// <summary>
        /// Толщина линий ресайза
        /// </summary>
        public Thickness LinesThickness
        {
            get { return _linesThickness; }
            set
            {
                if (LinesThickness.Equals(value)) return;
                _linesThickness = value;
                UpdateThickness();
            }
        }

        /// <summary>
        /// Устанавливаем толщины линий
        /// </summary>
        public void UpdateThickness()
        {
            Left.Width = LinesThickness.Left;
            Right.Width = LinesThickness.Right;
            Top.Height = LinesThickness.Top;
            Bottom.Height = LinesThickness.Bottom;
        }

        #endregion

        #region CORNERS

        public TCorner BottomLeft;
        public TCorner BottomRight;

        /// <summary>
        /// Углы
        /// </summary>
        public SmartCollection<TCorner> Corners;

        public TCorner TopLeft;
        public TCorner TopRight;

        #endregion

        // public SimpleSubject<FrameworkElement> ActivationSubject { get; set; }

        public virtual void Activate(FlexCanvas host, FrameworkElement target = null)
        {
            Host = host;
            Target = target;
            AddChildren();
            // this.ActivationSubject.OnNext(this.Target);
        }


        public virtual void Deactivate()
        {
            RemoveChildren();
            Target = null;
            Host = null;
        }


        public virtual void PaintSides()
        {
            foreach (TSide side in Sides) side.Opacity = LinesOpacity;
        }

        public virtual Boundary<TSide, TCorner> AddChildren()
        {
            foreach (TSide side in Sides) Host.AddChild(side);
            foreach (TCorner corner in Corners) Host.AddChild(corner);
            return this;
        }

        public virtual Boundary<TSide, TCorner> RemoveChildren()
        {
            foreach (TSide side in Sides) Host.RemoveChild(side);
            foreach (TCorner corner in Corners) Host.RemoveChild(corner);
            return this;
        }

        #region SUBSCRIPTIONS AND UNSUBSCRIPTIONS

        protected void Subscribe(FrameworkElement element)
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> subject = element.GetOnArrange() ??
                                                                        new SimpleSubject
                                                                            <Args<FrameworkElement, Rect, Size>>();
            subject.DoOnNext += LeftOnArrange;
            subject.DoOnNext += RightOnArrange;
            subject.DoOnNext += TopOnArrange;
            subject.DoOnNext += BottomOnArrange;

            subject.DoOnNext += TopLeftOnArrange;
            subject.DoOnNext += TopRightOnArrange;
            subject.DoOnNext += BottomLeftOnArrange;
            subject.DoOnNext += BottomRightOnArrange;
            element.SetOnArrange(subject);
        }

        protected void UnSubscribe(FrameworkElement element)
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> subject = element.GetOnArrange();
            if (subject == null) return;
            subject.DoOnNext -= LeftOnArrange;
            subject.DoOnNext -= RightOnArrange;
            subject.DoOnNext -= TopOnArrange;
            subject.DoOnNext -= BottomOnArrange;

            subject.DoOnNext -= TopLeftOnArrange;
            subject.DoOnNext -= TopRightOnArrange;
            subject.DoOnNext -= BottomLeftOnArrange;
            subject.DoOnNext -= BottomRightOnArrange;
            element.SetOnArrange(subject);
        }

        #endregion

        #region SIDES ONARRANGE FUNCTIONS

        protected Rect ExtractSlot(Args<FrameworkElement, Rect, Size> value)
        {
            return Target == Host ? new Rect(0, 0, value.Param1.Width, value.Param1.Height) : value.Param1;
        }

        public virtual void LeftOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            slot.Width = LinesThickness.Left;
            slot.X = slot.X - slot.Width;
            Left.SetPlace(slot);
        }


        public virtual void RightOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            slot.X = slot.Right;
            slot.Width = LinesThickness.Right;
            Right.SetPlace(slot);
        }

        public virtual void TopOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            slot.Height = LinesThickness.Top;
            slot.Y = slot.Y - slot.Height;
            Top.SetPlace(slot);
        }

        public virtual void BottomOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            slot.Y = slot.Bottom;
            slot.Height = LinesThickness.Bottom;
            Bottom.SetPlace(slot);
        }

        #endregion

        #region CORNERS ONARRANGE FUNCTIONS

        public virtual void TopLeftOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            TopLeft.SetPos(new Pos3D(slot.X, slot.Y, 0, true));
        }

        public virtual void TopRightOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            TopRight.SetPos(new Pos3D(slot.Right, slot.Y, 0, true));
        }

        public virtual void BottomLeftOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            BottomLeft.SetPos(new Pos3D(slot.X, slot.Bottom, 0, true));
        }

        public virtual void BottomRightOnArrange(Args<FrameworkElement, Rect, Size> value)
        {
            Rect slot = ExtractSlot(value);
            BottomRight.SetPos(new Pos3D(slot.Right, slot.Bottom, 0, true));
        }

        #endregion
    }
}